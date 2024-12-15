using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi.Data;
using WebApi.Data.DataBase;
using WebApi.Data.Requests;

namespace WebApi;

public class Program
{
    /// <summary>
    /// Точка входа в приложение.
    /// Конфигурирует и запускает Web API приложение, настраивая все необходимые сервисы и маршруты.
    /// </summary>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Добавляем сервисы в контейнер.
        builder.Services.AddDbContext<ApplicationContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        // Добавляем поддержку OpenAPI (Swagger)
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();


        // Конфигурация HTTP-пайплайна
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Включение поддержки HTTPS
        app.UseHttpsRedirection();

        // **1. Добавление нового заказа**
        app.MapPost("/api/orders", async ([FromBody] CreateOrderRequest request, ApplicationContext db) =>
        {
            var order = new Order
            {
                TotalAmount = request.TotalAmount,
                PaidAmount = request.PaidAmount,
                OrderDate = DateTime.UtcNow,
            };
            db.Orders.Add(order);
            await db.SaveChangesAsync();
            return Results.Ok(order);
        });

        // **2. Получение списка заказов**
        app.MapGet("/api/orders", async (ApplicationContext db) =>
        {
            var orders = await db.Orders.ToListAsync();
            return Results.Ok(orders);
        });

        // **3. Получение заказа по ID**
        app.MapGet("/api/orders/{id}", async ([FromRoute(Name = "id")] int OrderID, ApplicationContext db) => {
            var orders = await db.Orders.FirstOrDefaultAsync(o => o.OrderID == OrderID);
            return Results.Ok(orders);
        });

        // **4. Добавление прихода денег**
        app.MapPost("/api/incomes", async ([FromBody] CreateIncomeRequest request, ApplicationContext db) =>
        {
            var income = new MoneyIncome
            {
                IncomeDate = DateTime.UtcNow,
                TotalAmount = request.TotalAmount,
                RemainingAmount = request.RemainingAmount,
            };
            db.MoneyIncome.Add(income);
            await db.SaveChangesAsync();
            return Results.Ok(income);
        });

        // **5. Получение списка приходов денег**
        app.MapGet("/api/incomes", async (ApplicationContext db) => {
            var incomes = await db.MoneyIncome.ToListAsync();
            return Results.Ok(incomes);
        });

        // **6. Получение прихода денег по ID**
        app.MapGet("/api/incomes/{id}", async ([FromRoute(Name = "id")] int incomeID, ApplicationContext db) => {
            var incomes = await db.MoneyIncome.FirstOrDefaultAsync(i => i.IncomeID == incomeID);
            return Results.Ok(incomes);
        });

        // **7. Добавление платежа**
        app.MapPost("/api/payments", async ([FromBody] CreatePaymentRequest request, ApplicationContext db) =>
        {
            // Получаем заказ и приход по ID
            var order = await db.Orders.FindAsync(request.OrderID);
            var income = await db.MoneyIncome.FindAsync(request.IncomeID);

            if (order == null || income == null)
                return Results.NotFound("Заказ или денежный приход не найдены.");

            if (income.RemainingAmount != request.ExpectedRemainingAmount) {
                return Results.Conflict("Обновите информацию платежей.");
            }

            if (order.PaidAmount != request.ExpectedPaidAmount) {
                return Results.Conflict("Обновите информацию по заказам.");
            }

            if (income.RemainingAmount < request.PaymentAmount)
                return Results.BadRequest("Недостаточно средств в выбранном приходе.");

            if (order.TotalAmount - order.PaidAmount < request.PaymentAmount)
                return Results.BadRequest("Осталось оплатить меньше передаваемого.");

            var payment = new Payment
            {
                OrderID = request.OrderID,
                IncomeID = request.IncomeID,
                PaymentAmount = request.PaymentAmount,
            };
            db.Payments.Add(payment);
            await db.SaveChangesAsync();

            return Results.Ok(payment);
        });

        // **8. Получение списка платежей**
        app.MapGet("/api/payments", async ([FromQuery(Name = "orderId")] int? orderID, ApplicationContext db) => {
            IQueryable<Payment> query = db.Payments.AsQueryable();
            if (orderID != null) {
                query = query.Where(p => p.OrderID == orderID);
            }
            var payments = await query.ToListAsync();
            return Results.Ok(payments);
        });

        app.Run();
    }
}
