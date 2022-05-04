# Database design and SQL (DQL). Subqueries

## Task  

1. Get product list, which price is two  times higher or more then minimum price (id, product, price), sorted ascending by price, product. Scheme of result data set: **id| product | price**

1. Get a list of customers with birthday date range inclusive 1 Jan 2000 – 31 Dec 2010 (date format is YYYY-MM-DD), whom total sum of purchases is higher then average total purchase sum of all authorized customers (last name, first name, total expenses). Sort ascending by total expenses, last name. Scheme of result data set: **surname | name | total_expenses**
 
1. Get orders for 2021 year, which contain more items per order (not items’ product amount) than the average number of items across all orders. Sort ascendidng by order items amount and order id. Scheme of result data set: **order_id |  items_count**



### Domain description   

Supermarkets sell goods of various categories. The customers can shop anonymously or by logging in. When buying, a receipt is created with a list of goods purchased in a particular market. 

![DBSchema](/Subqueries/sql_queries/DBSchema.jpg)

### How to complete task solution

Save the script with the query for subtask 1 to file **sql_queries / task1.sql**, the next one to file **sql_queries / task2.sql**, etc. 
______
