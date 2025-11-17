-- ==========================================================
-- RENTAL_SERVICE SCHEMA 
-- ==========================================================

-- Create schema
CREATE SCHEMA IF NOT EXISTS rental_service AUTHORIZATION postgres;


-- ==========================================================
-- AUDIT_LOG TABLE
-- ==========================================================

CREATE TABLE IF NOT EXISTS rental_service."audit_log" (
    id BIGSERIAL PRIMARY KEY,
	correlation_id UUID NULL,
    event_type TEXT,
    message TEXT,
    object_before TEXT,
    object_after TEXT,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    username TEXT
);

CREATE INDEX IF NOT EXISTS idx_audit_log_created_at ON rental_service."audit_log"(created_at);
CREATE INDEX IF NOT EXISTS idx_audit_log_username ON rental_service."audit_log"(username);
CREATE INDEX IF NOT EXISTS idx_audit_log_correlation_id ON rental_service."audit_log"(correlation_id);


-- ==========================================================
-- REFRESH_TOKENS TABLE
-- ==========================================================
CREATE TABLE IF NOT EXISTS rental_service."refresh_tokens" (
    "id" UUID PRIMARY KEY,
    "username" text,
    "token" UUID NOT NULL,
    "expiration_date" timestamptz NOT NULL
);


-- ==========================================================
-- ROLES TABLE
-- ==========================================================
CREATE TABLE IF NOT EXISTS rental_service."roles" (
    "id" UUID PRIMARY KEY,
    "name" varchar(256),
    "normalized_name" varchar(256) UNIQUE,
    "concurrency_stamp" text
);


-- ==========================================================
-- USERS TABLE
-- ==========================================================
CREATE TABLE IF NOT EXISTS rental_service."users" (
    "id" UUID PRIMARY KEY,
    "user_name" varchar(256),
    "normalized_username" varchar(256) UNIQUE,
    "email" varchar(256),
    "normalized_email" varchar(256),
    "email_confirmed" boolean NOT NULL,
    "password_hash" text,
    "security_stamp" text,
    "concurrency_stamp" text,
    "phone_number" text,
    "phone_number_confirmed" boolean NOT NULL,
    "two_factor_enabled" boolean NOT NULL,
    "lockout_end" timestamptz,
    "lockout_enabled" boolean NOT NULL,
    "access_failed_count" int NOT NULL
);

CREATE INDEX IF NOT EXISTS "ix_users_email" ON rental_service."users"("normalized_email");
CREATE INDEX IF NOT EXISTS "ix_user_name" ON rental_service."users"("normalized_username");


-- ==========================================================
-- ROLE_CLAIMS TABLE
-- ==========================================================
CREATE TABLE IF NOT EXISTS rental_service."role_claims" (
    "id" SERIAL PRIMARY KEY,
    "role_id" UUID NOT NULL,
    "claim_type" text,
    "claim_value" text,
    CONSTRAINT "fk_role_claims_roles_role_id" FOREIGN KEY ("role_id") REFERENCES rental_service."roles"("id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "ix_role_claims_role_id" ON rental_service."role_claims"("role_id");


-- ==========================================================
-- USER_CLAIMS TABLE
-- ==========================================================
CREATE TABLE IF NOT EXISTS rental_service."user_claims" (
    "id" SERIAL PRIMARY KEY,
    "user_id" UUID NOT NULL,
    "claim_type" text,
    "claim_value" text,
    CONSTRAINT "fk_user_claims_users_user_id" FOREIGN KEY ("user_id") REFERENCES rental_service."users"("id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "ix_user_claims_user_id" ON rental_service."user_claims"("user_id");


-- ==========================================================
-- USER_LOGINS TABLE
-- ==========================================================
CREATE TABLE IF NOT EXISTS rental_service."user_logins" (
    "login_provider" varchar(128) NOT NULL,
    "provider_key" varchar(128) NOT NULL,
    "provider_display_name" text,
    "user_id" UUID NOT NULL,
    PRIMARY KEY ("login_provider", "provider_key"),
    CONSTRAINT "fk_user_logins_users_user_id" FOREIGN KEY ("user_id") REFERENCES rental_service."users"("id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "ix_user_logins_user_id" ON rental_service."user_logins"("user_id");


-- ==========================================================
-- USER_ROLES TABLE
-- ==========================================================
CREATE TABLE IF NOT EXISTS rental_service."user_roles" (
    "user_id" UUID NOT NULL,
    "role_id" UUID NOT NULL,
    PRIMARY KEY ("user_id", "role_id"),
    CONSTRAINT "fk_user_roles_users_user_id" FOREIGN KEY ("user_id") REFERENCES rental_service."users"("id") ON DELETE CASCADE,
    CONSTRAINT "fk_user_roles_roles_role_id" FOREIGN KEY ("role_id") REFERENCES rental_service."roles"("id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "ix_user_roles_role_id" ON rental_service."user_roles"("role_id");


-- ==========================================================
-- USER_TOKENS TABLE
-- ==========================================================
CREATE TABLE IF NOT EXISTS rental_service."user_tokens" (
    "user_id" UUID NOT NULL,
    "login_provider" varchar(128) NOT NULL,
    "name" varchar(128) NOT NULL,
    "value" text,
    PRIMARY KEY ("user_id", "login_provider", "name"),
    CONSTRAINT "fk_user_tokens_users_user_id" FOREIGN KEY ("user_id") REFERENCES rental_service."users"("id") ON DELETE CASCADE
);


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


-- ==========================================================
-- INSERT USERS 
-- ==========================================================
INSERT INTO rental_service."users" 
(id, user_name, normalized_username, email, normalized_email, email_confirmed, password_hash, security_stamp, concurrency_stamp, phone_number_confirmed, two_factor_enabled, lockout_enabled, access_failed_count)
VALUES
(gen_random_uuid(), 'admin@rental.com', 'ADMIN@RENTAL.COM', 'admin@rental.com', 'ADMIN@RENTAL.COM', true, 'AQAAAAIAAYagAAAAEHxCbOqEDXrSFqZQSCfynqxY2wtgr1ElLdSyLrGfX9T8RxdGaffs29y6s7ucsX1Q3g==', gen_random_uuid(), gen_random_uuid(), false, false, false, 0),
(gen_random_uuid(), 'manager@rental.com', 'MANAGER@RENTAL.COM', 'manager@rental.com', 'MANAGER@RENTAL.COM', true, 'AQAAAAIAAYagAAAAEO1UnZOn8v4us/0Mp839/lbVwFwMB3NnKk0mx4bB4ITTEjYmGshkUgCTGqgsgr+TSA==', gen_random_uuid(), gen_random_uuid(), false, false, false, 0),
(gen_random_uuid(), 'user@rental.com', 'USER@RENTAL.COM', 'user@rental.com', 'USER@RENTAL.COM', true, 'AQAAAAIAAYagAAAAEO0lQEG6wMBE9buJqO0vIQlq6Hkv9rYbhjTGYNaoC0osDAHR2K5dTZB05+H8SluQqA==', gen_random_uuid(), gen_random_uuid(), false, false, false, 0);


-- ==========================================================
-- INSERT ROLES (Admin, Manager, User)
-- ==========================================================
INSERT INTO rental_service.roles (id, name, normalized_name, concurrency_stamp)
VALUES 
  (gen_random_uuid(), 'Admin', 'ADMIN', gen_random_uuid()),
  (gen_random_uuid(), 'Manager', 'MANAGER', gen_random_uuid()),
  (gen_random_uuid(), 'User', 'USER', gen_random_uuid());


-- ==========================================================
-- INSERT USER_ROLES 
-- ==========================================================
INSERT INTO rental_service."user_roles" (user_id, role_id)
VALUES (
  (SELECT id FROM rental_service.users WHERE email = 'admin@rental.com'),
  (SELECT id FROM rental_service.roles WHERE name = 'Admin')
);

INSERT INTO rental_service."user_roles" (user_id, role_id)
VALUES (
  (SELECT id FROM rental_service.users WHERE email = 'manager@rental.com'),
  (SELECT id FROM rental_service.roles WHERE name = 'Manager')
);

INSERT INTO rental_service."user_roles" (user_id, role_id)
VALUES (
  (SELECT id FROM rental_service.users WHERE email = 'user@rental.com'),
  (SELECT id FROM rental_service.roles WHERE name = 'User')
);


-- ==========================================================
-- INSERT DRIVER_LICENSE_TYPE
-- ==========================================================
INSERT INTO rental_service."driver_license_type" (id, code, description)
VALUES 
    (gen_random_uuid(), 'A',  'Motorcycle'),
    (gen_random_uuid(), 'B',  'Car'),
    (gen_random_uuid(), 'AB', 'Motorcycle and Car');



