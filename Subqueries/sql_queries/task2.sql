SELECT
    person.surname,
    person.name,
    SUM(order_details.price_with_discount * order_details.product_amount) AS total_expenses
FROM person
INNER JOIN customer
    ON person.id = customer.person_id
INNER JOIN customer_order
    ON customer.person_id = customer_order.customer_id
INNER JOIN order_details ON
    customer_order.id = order_details.customer_order_id
WHERE
    customer.card_number IS NOT NULL AND person.birth_date BETWEEN '2000-01-01' AND '2010-12-31'
GROUP BY person.id
HAVING SUM(order_details.price_with_discount * order_details.product_amount) > (
    SELECT
        AVG(total_expenses)
    FROM (
        SELECT
            SUM(order_details.price_with_discount * order_details.product_amount) AS total_expenses
        FROM
            customer
        INNER JOIN customer_order
            ON customer.person_id = customer_order.customer_id
        INNER JOIN order_details
            ON customer_order.id = order_details.customer_order_id
        WHERE
            customer.card_number IS NOT NULL
        GROUP BY customer.person_id
    ) AS subquery
)
ORDER BY total_expenses, person.surname;
