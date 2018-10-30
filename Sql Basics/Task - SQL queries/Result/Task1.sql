select O.CustomerID, SUM(OD.UnitPrice * OD.Quantity * (1 - OD.Discount)) as OrderSum from Customers as C
join Orders as O
on C.CustomerID = O.CustomerID
join [Order Details] as OD
on O.OrderID = OD.OrderID
group by O.CustomerID
order by OrderSum desc