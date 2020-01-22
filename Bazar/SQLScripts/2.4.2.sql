CREATE OR REPLACE VIEW `active_contracts` AS
    SELECT 
        *
    FROM
        contracts
    WHERE
        ((contracts.cancel_date IS NULL
            AND CURDATE() BETWEEN contracts.start_date AND contracts.end_date)
            OR (contracts.cancel_date IS NOT NULL
            AND CURDATE() BETWEEN contracts.start_date AND contracts.cancel_date));
