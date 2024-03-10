SELECT
    customer_order.id AS order_id,
    COUNT(order_details.product_id) AS items_count
FROM customer_order
INNER JOIN order_details
    ON customer_order.id = order_details.customer_order_id
WHERE customer_order.operation_time BETWEEN '2021-01-01' AND '2021-12-31'
GROUP BY customer_order.id
HAVING COUNT(order_details.product_id) > (
    SELECT AVG(item_count)
    FROM (
        SELECT
            COUNT(order_details.product_id) AS item_count
        FROM customer_order
        INNER JOIN order_details
            ON customer_order.id = order_details.customer_order_id
        GROUP BY customer_order.id
    ) AS subquery
)
ORDER BY items_count, order_id