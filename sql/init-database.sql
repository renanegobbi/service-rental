-- ==========================================================
-- SCHEMA RENTAL
-- ==========================================================

-- Create schema
CREATE SCHEMA IF NOT EXISTS rental_service AUTHORIZATION postgres;


-- ==========================================================
-- DRIVER_LICENSE_TYPE TABLE
-- ==========================================================
CREATE TABLE IF NOT EXISTS rental_service."driver_license_type" (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    code VARCHAR(5) NOT NULL,         -- e.g. 'A', 'B', 'AB'
    description VARCHAR(100) NOT NULL,       -- e.g. 'Motorcycle', 'Car', 'Motorcycle and Car'
    is_active BOOLEAN NOT NULL DEFAULT TRUE, -- allows deactivation without deleting
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_license_type_code ON rental_service."driver_license_type"(code);


-- ==========================================================
-- COURIER TABLE
-- ==========================================================
CREATE TABLE rental_service."courier" (
    id UUID PRIMARY KEY,
    full_name VARCHAR(100) NOT NULL,
    cnpj VARCHAR(18) NOT NULL UNIQUE,
    birth_date DATE NOT NULL,
    driver_license_number VARCHAR(20) NOT NULL UNIQUE,
    driver_license_type_id UUID NOT NULL REFERENCES rental_service."driver_license_type"(id)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,
    driver_license_image_url TEXT,
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_driver_name ON rental_service."courier" (full_name);


-- ==========================================================
-- MOTORCYCLE TABLE
-- ==========================================================
CREATE TABLE rental_service."motorcycle" (
    id UUID PRIMARY KEY,
    year INTEGER NOT NULL CHECK (year > 2000),
    model VARCHAR(50) NOT NULL,
    plate VARCHAR(10) NOT NULL UNIQUE,
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE INDEX idx_motorcycle_model ON rental_service."motorcycle" (model);


-- ==========================================================
-- RENTAL_PLAN TABLE
-- ==========================================================

-- Create table rental_plan
CREATE TABLE IF NOT EXISTS  rental_service."rental_plan" (
    id UUID PRIMARY KEY,
    days INTEGER NOT NULL CHECK (days > 0),
    daily_rate NUMERIC(10,2) NOT NULL CHECK (daily_rate > 0),
    penalty_percent NUMERIC(5,2) NOT NULL DEFAULT 0,
    description VARCHAR(100),
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Index
CREATE INDEX IF NOT EXISTS "idx_plan_days" ON rental_service."rental_plan"("days");


-- ==========================================================
-- RENTAL TABLE
-- ==========================================================
CREATE TABLE IF NOT EXISTS rental_service."rental" (
    id UUID PRIMARY KEY,
    driver_id UUID NOT NULL REFERENCES rental_service."courier"(id)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,
    motorcycle_id UUID NOT NULL REFERENCES rental_service."motorcycle"(id)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,
    plan_id UUID NOT NULL REFERENCES rental_service."rental_plan"(id)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,
    start_date DATE NOT NULL,
    expected_end_date DATE NOT NULL,
    end_date DATE,
    daily_value NUMERIC(10,2),
    penalty_value NUMERIC(10,2),
    total_value NUMERIC(10,2),
    status VARCHAR(20) NOT NULL DEFAULT 'ACTIVE'
        CHECK (status IN ('ACTIVE', 'FINISHED', 'CANCELLED')),
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    CONSTRAINT unique_active_rental_per_motorcycle UNIQUE (motorcycle_id, status)
);


-- ==========================================================
-- NOTIFICATION TABLE
-- ==========================================================
CREATE TABLE rental_service."notification" (
    id UUID PRIMARY KEY,
    event_type VARCHAR(100) NOT NULL,             
    payload JSONB NOT NULL,                       
    status VARCHAR(20) DEFAULT 'PENDING'          -- PENDING | SENT | ERROR
        CHECK (status IN ('PENDING', 'SENT', 'ERROR')),
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    sent_at TIMESTAMP
);



-- Insert driver license type
INSERT INTO rental_service."driver_license_type" (id, code, description)
VALUES 
    (gen_random_uuid(), 'A',  'Motorcycle'),
    (gen_random_uuid(), 'B',  'Car'),
    (gen_random_uuid(), 'AB', 'Motorcycle and Car');