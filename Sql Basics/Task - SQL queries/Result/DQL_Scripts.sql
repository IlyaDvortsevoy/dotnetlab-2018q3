USE Northwind;

--Task1
SELECT O.CustomerID, SUM(OD.UnitPrice * OD.Quantity * (1 - OD.Discount)) AS OrderSum
FROM Customers AS C
join Orders AS O
ON C.CustomerID = O.CustomerID
join [Order Details] AS OD
ON O.OrderID = OD.OrderID
GROUP BY O.CustomerID
ORDER BY OrderSum DESC;

--Task2
--without grouping
SELECT C.CustomerID, C.CompanyName AS CustomerName, S.SupplierID, S.CompanyName AS SupplierName
FROM Customers AS C
LEFT JOIN Suppliers AS S
ON C.City = S.City AND C.Country = S.Country
ORDER BY C.CustomerID;

--with grouping
SELECT C.CustomerID, C.CompanyName AS CustomerName, COUNT(S.SupplierID) AS Match
FROM Customers AS C
LEFT JOIN Suppliers AS S
ON C.City = S.City AND C.Country = S.Country
GROUP BY C.CustomerID, C.CompanyName
ORDER BY C.CustomerID;

--Task3 
SELECT C.CompanyName, MAX(OD.UnitPrice * OD.Quantity * (1 - OD.Discount)) AS MaxOrderSum
FROM Customers AS C
INNER JOIN Orders AS O
ON C.CustomerID = O.CustomerID
INNER JOIN [Order Details] AS OD
ON O.OrderID = OD.OrderID
GROUP BY C.CompanyName
HAVING MAX(OD.UnitPrice * OD.Quantity * (1 - OD.Discount)) > 500
ORDER BY MAX(OD.UnitPrice * OD.Quantity * (1 - OD.Discount));

--Task4
SELECT	C.CustomerID, 
		C.CompanyName, 
		YEAR(MIN(O.OrderDate)) AS InitialYear,
		MONTH(MIN(O.OrderDate)) AS InitialMonth
FROM Customers AS C
INNER JOIN Orders AS O
ON C.CustomerID = O.CustomerID
GROUP BY C.CustomerID, C.CompanyName;

--Task5
SELECT	C.CustomerID, 
		C.CompanyName, 
		YEAR(MIN(O.OrderDate)) AS InitialYear,
		MONTH(MIN(O.OrderDate)) AS InitialMonth, 
		SUM(OD.UnitPrice * OD.Quantity * (1 - OD.Discount)) AS OrderSum
FROM Customers AS C
INNER JOIN Orders AS O
ON C.CustomerID = O.CustomerID
INNER JOIN [Order Details] AS OD
ON O.OrderID = OD.OrderID
GROUP BY C.CustomerID, C.CompanyName
ORDER BY InitialYear, InitialMonth, OrderSum DESC, C.CompanyName;

--Task6
SELECT CustomerID, CompanyName, ContactName, PostalCode, Region, Phone FROM Customers
WHERE	ISNUMERIC(PostalCode) = 0 OR
		Region IS NULL OR
		Phone NOT LIKE '(%';

--Task7
SELECT	C.CategoryName,
		CASE
			WHEN P.UnitsInStock > 0 Then 'True'
			WHEN P.UnitsInStock = 0 Then 'False'
		END AS OnStock,
		COUNT(P.ProductID) AS ProductCount, 
		SUM(P.UnitPrice * P.UnitsInStock) AS TotalSum
FROM Categories AS C
INNER JOIN Products AS P
ON C.CategoryID = P.CategoryID
GROUP BY C.CategoryName, CASE	WHEN P.UnitsInStock > 0 Then 'True'
								WHEN P.UnitsInStock = 0 Then 'False' END
ORDER BY TotalSum;
