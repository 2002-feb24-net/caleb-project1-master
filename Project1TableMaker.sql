--use Project0Db
--go
DROP TABLE IF EXISTS OrderItem;
DROP TABLE IF EXISTS Inventory;
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS Customers;
DROP TABLE IF EXISTS Stores;
DROP TABLE IF EXISTS Products;

CREATE TABLE Products (
ID int,
Name nvarchar(100),
Price money,
ImageURL nvarchar(500),
PRIMARY KEY (ID)
);
CREATE TABLE Stores (
ID int,
Address nvarchar(200),
PRIMARY KEY (ID)
);
CREATE TABLE Customers (
ID int,
Username nvarchar(50),
Password nvarchar(50),
FirstName nvarchar(200),
LastName nvarchar(100),
PRIMARY KEY (ID)
);
CREATE TABLE Orders (
ID int,
CustomerID int,
StoreID int,
Total money,
OrderTime datetime,
PRIMARY KEY (ID),
FOREIGN KEY (CustomerID) REFERENCES Customers(ID),
FOREIGN KEY (StoreID) REFERENCES Stores(ID)
);
CREATE TABLE Inventory(
ID int,
ProductID int,
StoreID int,
Quantity int,
PRIMARY KEY (ID),
FOREIGN KEY (ProductID) REFERENCES Products(ID),
FOREIGN KEY (StoreID) REFERENCES Stores(ID)
);
CREATE TABLE OrderItem (
ID int,
OrderID int,
Quantity int,
InventoryID int,
PRIMARY KEY (ID),
FOREIGN KEY (OrderID) REFERENCES Orders(ID),
FOREIGN KEY (InventoryID) REFERENCES Inventory(ID)
);
SELECT * FROM OrderItem;