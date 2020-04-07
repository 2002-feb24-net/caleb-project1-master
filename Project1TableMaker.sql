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
ProductID int,
InventoryID int,
PRIMARY KEY (ID),
FOREIGN KEY (OrderID) REFERENCES Orders(ID),
FOREIGN KEY (ProductID) REFERENCES Products(ID),
FOREIGN KEY (InventoryID) REFERENCES Inventory(ID)
);

--insert into tables
INSERT INTO Products (ID, Name, Price, ImageUrl)
VALUES (1, 'Book About Nothing', 11.95, 'bookaboutnothing.png'),
	   (2, 'Book About Something', 22.95, 'bookaboutsomething.png'),
       (3, 'Book About Everything', 33.95, 'bookabouteverything.png');
	   
INSERT INTO Stores (ID, Address)
VALUES (1, 'Dallas'),
       (2, 'Houston');
	   
INSERT INTO Customers (ID, Username, Password, FirstName, LastName)
VALUES (1, 'username1', 'password1', 'Amy', 'Actor'),
       (2, 'username2', 'password2', 'Bob', 'Builder'),
       (3, 'username3', 'password3', 'Charlie', 'Chaplin');
	   
INSERT INTO Orders (ID, CustomerID, StoreID, Total, OrderTime)
VALUES (1, 1, 1, 11.95, CURRENT_TIMESTAMP),
       (2, 2, 1, 33.95, CURRENT_TIMESTAMP),
       (3, 3, 2, 22.95, CURRENT_TIMESTAMP);
	   
INSERT INTO Inventory (ID, ProductID, StoreID, Quantity)
VALUES (1, 1, 1, 111),
       (2, 2, 1, 122),
       (3, 3, 1, 133),
       (4, 1, 2, 211),
       (5, 2, 2, 222),
       (6, 3, 2, 233);
	   
INSERT INTO OrderItem (ID, OrderID, Quantity, ProductID, InventoryID)
VALUES (1, 1, 1, 1, 1),
       (2, 2, 1, 3, 3),
       (3, 3, 1, 2, 5);
SELECT * FROM OrderItem;