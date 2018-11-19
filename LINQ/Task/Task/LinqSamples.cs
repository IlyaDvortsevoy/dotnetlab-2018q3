// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
	[Title("LINQ Module")]
	[Prefix("Linq")]
	public class LinqSamples : SampleHarness
	{

		private DataSource dataSource = new DataSource();

	    [Category("LINQ Module Tasks")]
	    [Title("Task 1")]
	    [Description("This sample gets a list of all customers whose total turnover (the sum of all orders) exceeds some value of X.")]
	    public void Linq1()
	    {
	        decimal turnover = 200M;

            var customers =
	            from customer in dataSource.Customers
                let sum = customer.Orders.Sum(order => order.Total)
                where sum > turnover
                select new
                {
                    ID = customer.CustomerID,
                    CompanyName = customer.CompanyName,
                    OrdersSum = sum
                };

	        Console.WriteLine("Query with \"turnover\" variable equaled to 200");

	        foreach (var customer in customers)
	        {
	            ObjectDumper.Write(customer);
	        }

	        turnover = 700M; // To avoid of copying the query several times

	        Console.WriteLine("Query with \"turnover\" variable equaled to 700");

            foreach (var customer in customers)
	        {
	            ObjectDumper.Write(customer);
	        }
        }

	    [Category("LINQ Module Tasks")]
	    [Title("Task 2 with grouping")]
	    [Description("This sample makes a list of suppliers located in the same country and the same city for each client using grouping.")]
	    public void Linq2()
	    {
	        var customerSuppliers =
	            from customer in dataSource.Customers
	            join supplier in dataSource.Suppliers on
	                new {customer.Country, customer.City} equals new {supplier.Country, supplier.City}
	                into match
	            from customerSupplier in match
	            select new
	            {
	                customer.CompanyName,
	                customer.Country,
	                customer.City,
	                customerSupplier.SupplierName
	            };

            foreach (var customerSupplier in customerSuppliers)
	        {
	            ObjectDumper.Write(customerSupplier);
	        }
	    }

	    [Category("LINQ Module Tasks")]
	    [Title("Task 2")]
	    [Description("This sample makes a list of suppliers located in the same country and the same city for each client without grouping.")]
	    public void Linq3()
	    {
	        var customerSuppliers =
	            from customer in dataSource.Customers
	            from supplier in dataSource.Suppliers
                where supplier.Country == customer.Country && supplier.City == customer.City
	            select new
	            {
	                customer.CompanyName,
	                customer.Country,
	                customer.City,
	                supplier.SupplierName
	            };

            foreach (var customerSupplier in customerSuppliers)
	        {
	            ObjectDumper.Write(customerSupplier);
	        }
	    }

	    [Category("LINQ Module Tasks")]
	    [Title("Task 3")]
	    [Description("This sample finds all customers who have orders that exceed the sum of X.")]
	    public void Linq4()
	    {
	        decimal orderTotal = 1500M;

	        var customers =
	            from customer in dataSource.Customers
	            from order in customer.Orders
	            where order.Total > orderTotal
                orderby customer.CompanyName
                select new
	            {
	                Company = customer.CompanyName,
	                order.OrderID,
                    order.Total
	            };

	        foreach (var customer in customers)
	        {
	            ObjectDumper.Write(customer);
	        }
	    }

	    [Category("LINQ Module Tasks")]
	    [Title("Task 4")]
	    [Description("This sample gets a list of customers including the month of the year they became clients.")]
	    public void Linq5()
	    {
	        var customers =
	            from customer in dataSource.Customers
	            where customer.Orders.Count() > 0
                let minDate = customer.Orders.Min(order => order.OrderDate)
                select new
	            {
	                Company = customer.CompanyName,
                    ClientFrom = minDate
                };

            foreach (var customer in customers)
            {
                ObjectDumper.Write(customer);
            }
        }

	    [Category("LINQ Module Tasks")]
	    [Title("Task 5")]
	    [Description("This sample gets a list of customers including the month of the year they became clients sorted by year, month, customer turnover (from the maximum to the minimum), and the client's name.")]
	    public void Linq6()
	    {
	        var customers =
	            from customer in dataSource.Customers
	            where customer.Orders.Count() > 0
	            let minDate = customer.Orders.Min(order => order.OrderDate)
                let turnover = customer.Orders.Sum(order => order.Total)
                orderby minDate.Year, minDate.Month, turnover descending, customer.CompanyName
                select new
	            {
	                Company = customer.CompanyName,
	                ClientFrom = minDate,
	                Turnover = turnover
                };

	        foreach (var customer in customers)
	        {
	            ObjectDumper.Write(customer);
	        }
	    }

	    [Category("LINQ Module Tasks")]
	    [Title("Task 6")]
	    [Description("This sample specifies all customers who have a non-digital postal code, or the region is not filled, or the operator code is not specified in the phone.")]
	    public void Linq7()
	    {
	        string template = @"\d*";
	        var regex = new Regex(template);

	        var customers =
	            from customer in dataSource.Customers
	            let validPostalCode = customer.PostalCode != null && regex.IsMatch(customer.PostalCode)
	            where validPostalCode == false ||
	                  customer.Region == null ||
	                  customer?.Phone[0] != '('
	            select new
	            {
	                Company = customer.CompanyName,
                    Region = customer.Region,
                    PostalCode = customer.PostalCode,
                    Phone = customer.Phone
	            };

	        foreach (var customer in customers)
	        {
	            ObjectDumper.Write(customer);
	        }
        }

	    [Category("LINQ Module Tasks")]
	    [Title("Task 7")]
	    [Description("This sample groups all products by category, inside - by stock, within the last group sort by cost.")]
	    public void Linq8()
	    {
	        var products =
	            from product in dataSource.Products
	            group product by product.Category into categoryGroup
                select new
                {
                    Category = categoryGroup.Key,
                    Products =
                        from product in categoryGroup
                        group product by product.UnitsInStock > 0 into inStock
                        select new
                        {
                            Status = inStock.Key,
                            Product =
                                from product in inStock
                                orderby product.UnitPrice
                                select new { Product = product.ProductName, Count = product.UnitsInStock, Price = product.UnitPrice }
                        }
                };

            foreach (var item in products)
            {
                ObjectDumper.Write($"---{item.Category}");
                Console.WriteLine();

                foreach (var entity in item.Products)
                {
                    string status = entity.Status ? "_InStock" : "_NotInStock";

                    ObjectDumper.Write(status);
                    Console.WriteLine();

                    foreach (var product in entity.Product)
                    {
                        ObjectDumper.Write(product);
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
            }
	    }
    }
}
