-- Создаем базу данных
CREATE DATABASE PaymentSystemDB;

-- Используем базу данных
USE PaymentSystemDB;

-- Таблица заказов
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),			-- Уникальный идентификатор заказа
    OrderDate DATE NOT NULL,						-- Дата заказа
    TotalAmount DECIMAL(10, 2) NOT NULL,			-- Сумма заказа
    PaidAmount DECIMAL(10, 2) NOT NULL DEFAULT 0	-- Оплаченная сумма
);

-- Таблица прихода денег
CREATE TABLE MoneyIncome (
    IncomeID INT PRIMARY KEY IDENTITY(1,1),			-- Уникальный идентификатор прихода денег
    IncomeDate DATE NOT NULL,						-- Дата прихода денег
    TotalAmount DECIMAL(10, 2) NOT NULL,			-- Сумма прихода
    RemainingAmount DECIMAL(10, 2) NOT NULL			-- Остаток
);

-- Таблица платежей
CREATE TABLE Payments (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),				-- Уникальный идентификатор платежа
    OrderID INT NOT NULL,									-- Ссылка на заказ
    IncomeID INT NOT NULL,									-- Ссылка на приход денег
    PaymentAmount DECIMAL(10, 2) NOT NULL,					-- Сумма платежа
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),		-- Связь с таблицей заказов
    FOREIGN KEY (IncomeID) REFERENCES MoneyIncome(IncomeID) -- Связь с таблицей прихода денег
);

-- Триггер для обновления суммы оплаты заказа и остатка прихода денег
CREATE TRIGGER UpdateOrderAndIncome
ON Payments
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Обновляем оплаченные суммы заказа
    UPDATE Orders
    SET PaidAmount = PaidAmount + inserted.PaymentAmount
    FROM Orders
    INNER JOIN inserted ON Orders.OrderID = inserted.OrderID;

    -- Обновляем остаток прихода денег
    UPDATE MoneyIncome
    SET RemainingAmount = RemainingAmount - inserted.PaymentAmount
    FROM MoneyIncome
    INNER JOIN inserted ON MoneyIncome.IncomeID = inserted.IncomeID;
END;

--Добавление заказов
INSERT INTO Orders (OrderDate, TotalAmount) VALUES
('2024-01-01', 1000.00),
('2024-01-05', 500.00),
('2024-01-10', 300.00);

--Добавление прихода денег
INSERT INTO MoneyIncome (IncomeDate, TotalAmount, RemainingAmount) VALUES
('2024-01-02', 1500.00, 1500.00),
('2024-01-06', 700.00, 700.00);

--Добавление платежей
INSERT INTO Payments (OrderID, IncomeID, PaymentAmount) VALUES
(1, 1, 500.00); -- Оплата заказа 1 с прихода 1

SELECT * FROM Orders;

SELECT * FROM MoneyIncome;

SELECT * FROM Payments;

Drop Table Payments, Orders, MoneyIncome;
Drop Database PaymentSystemDB;