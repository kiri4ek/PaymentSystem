-- ������� ���� ������
CREATE DATABASE PaymentSystemDB;

-- ���������� ���� ������
USE PaymentSystemDB;

-- ������� �������
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),			-- ���������� ������������� ������
    OrderDate DATE NOT NULL,						-- ���� ������
    TotalAmount DECIMAL(10, 2) NOT NULL,			-- ����� ������
    PaidAmount DECIMAL(10, 2) NOT NULL DEFAULT 0	-- ���������� �����
);

-- ������� ������� �����
CREATE TABLE MoneyIncome (
    IncomeID INT PRIMARY KEY IDENTITY(1,1),			-- ���������� ������������� ������� �����
    IncomeDate DATE NOT NULL,						-- ���� ������� �����
    TotalAmount DECIMAL(10, 2) NOT NULL,			-- ����� �������
    RemainingAmount DECIMAL(10, 2) NOT NULL			-- �������
);

-- ������� ��������
CREATE TABLE Payments (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),				-- ���������� ������������� �������
    OrderID INT NOT NULL,									-- ������ �� �����
    IncomeID INT NOT NULL,									-- ������ �� ������ �����
    PaymentAmount DECIMAL(10, 2) NOT NULL,					-- ����� �������
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),		-- ����� � �������� �������
    FOREIGN KEY (IncomeID) REFERENCES MoneyIncome(IncomeID) -- ����� � �������� ������� �����
);

-- ������� ��� ���������� ����� ������ ������ � ������� ������� �����
CREATE TRIGGER UpdateOrderAndIncome
ON Payments
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- ��������� ���������� ����� ������
    UPDATE Orders
    SET PaidAmount = PaidAmount + inserted.PaymentAmount
    FROM Orders
    INNER JOIN inserted ON Orders.OrderID = inserted.OrderID;

    -- ��������� ������� ������� �����
    UPDATE MoneyIncome
    SET RemainingAmount = RemainingAmount - inserted.PaymentAmount
    FROM MoneyIncome
    INNER JOIN inserted ON MoneyIncome.IncomeID = inserted.IncomeID;
END;

--���������� �������
INSERT INTO Orders (OrderDate, TotalAmount) VALUES
('2024-01-01', 1000.00),
('2024-01-05', 500.00),
('2024-01-10', 300.00);

--���������� ������� �����
INSERT INTO MoneyIncome (IncomeDate, TotalAmount, RemainingAmount) VALUES
('2024-01-02', 1500.00, 1500.00),
('2024-01-06', 700.00, 700.00);

--���������� ��������
INSERT INTO Payments (OrderID, IncomeID, PaymentAmount) VALUES
(1, 1, 500.00); -- ������ ������ 1 � ������� 1

SELECT * FROM Orders;

SELECT * FROM MoneyIncome;

SELECT * FROM Payments;

Drop Table Payments, Orders, MoneyIncome;
Drop Database PaymentSystemDB;