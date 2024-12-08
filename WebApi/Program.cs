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
    /// ����� ����� � ����������.
    /// ������������� � ��������� Web API ����������, ���������� ��� ����������� ������� � ��������.
    /// </summary>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ��������� ������� � ���������.
        builder.Services.AddDbContext<ApplicationContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        // ��������� ��������� OpenAPI (Swagger)
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();


        // ������������ HTTP-���������
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // ��������� ��������� HTTPS
        app.UseHttpsRedirection();

        // **1. ���������� ������ ������**
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

        // **2. ��������� ������ �������**
        app.MapGet("/api/orders", async (ApplicationContext db) =>
        {
            var orders = await db.Orders.ToListAsync();
            return Results.Ok(orders);
        });

        // **3. ��������� ������ �� ID**
        app.MapGet("/api/orders/{id}", async ([FromRoute(Name = "id")] int OrderID, ApplicationContext db) => {
            var orders = await db.Orders.FirstOrDefaultAsync(o => o.OrderID == OrderID);
            return Results.Ok(orders);
        });

        // **4. ���������� ������� �����**
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

        // **5. ��������� ������ �������� �����**
        app.MapGet("/api/incomes", async (ApplicationContext db) => {
            var incomes = await db.MoneyIncome.ToListAsync();
            return Results.Ok(incomes);
        });

        // **6. ��������� ������� ����� �� ID**
        app.MapGet("/api/incomes/{id}", async ([FromRoute(Name = "id")] int incomeID, ApplicationContext db) => {
            var incomes = await db.MoneyIncome.FirstOrDefaultAsync(i => i.IncomeID == incomeID);
            return Results.Ok(incomes);
        });

        // **7. ���������� �������**
        app.MapPost("/api/payments", async ([FromBody] CreatePaymentRequest request, ApplicationContext db) =>
        {
            // �������� ����� � ������ �� ID
            var order = await db.Orders.FindAsync(request.OrderID);
            var income = await db.MoneyIncome.FindAsync(request.IncomeID);

            if (order == null || income == null)
                return Results.NotFound("����� ��� �������� ������ �� �������.");

            if (income.RemainingAmount != request.ExpectedRemainingAmount) {
                return Results.Conflict("�������� ���������� ��������.");
            }

            if (order.PaidAmount != request.ExpectedPaidAmount) {
                return Results.Conflict("�������� ���������� �� �������.");
            }

            if (income.RemainingAmount < request.PaymentAmount)
                return Results.BadRequest("������������ ������� � ��������� �������.");

            if (order.TotalAmount - order.PaidAmount < request.PaymentAmount)
                return Results.BadRequest("�������� �������� ������ �������������.");

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

        // **8. ��������� ������ ��������**
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
