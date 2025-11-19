-- ==========================================================
-- RENTAL_SERVICE SCHEMA 
-- ==========================================================

CREATE SCHEMA IF NOT EXISTS rental_service AUTHORIZATION postgres;


-- ==========================================================
-- AUDIT_LOG TABLE
-- ==========================================================

CREATE TABLE IF NOT EXISTS rental_service.audit_log (
    id BIGSERIAL PRIMARY KEY,
    correlation_id UUID NULL,
    event_type TEXT,
    message TEXT,
    object_before TEXT,
    object_after TEXT,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    username TEXT
);

CREATE INDEX IF NOT EXISTS idx_audit_log_created_at ON rental_service.audit_log(created_at);
CREATE INDEX IF NOT EXISTS idx_audit_log_username ON rental_service.audit_log(username);
CREATE INDEX IF NOT EXISTS idx_audit_log_correlation_id ON rental_service.audit_log(correlation_id);


-- ==========================================================
-- REFRESH_TOKENS TABLE
-- ==========================================================

CREATE TABLE IF NOT EXISTS rental_service.refresh_tokens (
    id UUID PRIMARY KEY,
    username TEXT NOT NULL,
    token UUID NOT NULL,
    expiration_date TIMESTAMPTZ NOT NULL
);


-- ==========================================================
-- ROLES TABLE
-- ==========================================================

CREATE TABLE IF NOT EXISTS rental_service.roles (
    id UUID PRIMARY KEY,
    name VARCHAR(256) NOT NULL,
    normalized_name VARCHAR(256) NOT NULL,
    concurrency_stamp TEXT,
    CONSTRAINT uq_roles_name UNIQUE (name),
    CONSTRAINT uq_roles_normalized_name UNIQUE (normalized_name)
);


-- ==========================================================
-- USERS TABLE
-- ==========================================================

CREATE TABLE IF NOT EXISTS rental_service.users (
    id UUID PRIMARY KEY,
    user_name VARCHAR(256) NOT NULL,
    normalized_username VARCHAR(256) NOT NULL,
    email VARCHAR(256) NOT NULL,
    normalized_email VARCHAR(256) NOT NULL,
    email_confirmed BOOLEAN NOT NULL,
    password_hash TEXT,
    security_stamp TEXT,
    concurrency_stamp TEXT,
    phone_number TEXT,
    phone_number_confirmed BOOLEAN NOT NULL,
    two_factor_enabled BOOLEAN NOT NULL,
    lockout_end TIMESTAMPTZ,
    lockout_enabled BOOLEAN NOT NULL,
    access_failed_count INT NOT NULL,

    CONSTRAINT uq_users_username UNIQUE (user_name),
    CONSTRAINT uq_users_normalized_username UNIQUE (normalized_username),
    CONSTRAINT uq_users_email UNIQUE (email),
    CONSTRAINT uq_users_normalized_email UNIQUE (normalized_email)
);

CREATE INDEX IF NOT EXISTS ix_users_email ON rental_service.users (normalized_email);
CREATE INDEX IF NOT EXISTS ix_user_name ON rental_service.users (normalized_username);


-- ==========================================================
-- ROLE_CLAIMS TABLE
-- ==========================================================

CREATE TABLE IF NOT EXISTS rental_service.role_claims (
    id SERIAL PRIMARY KEY,
    role_id UUID NOT NULL,
    claim_type TEXT,
    claim_value TEXT,
    FOREIGN KEY (role_id) REFERENCES rental_service.roles(id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS ix_role_claims_role_id ON rental_service.role_claims (role_id);


-- ==========================================================
-- USER_CLAIMS TABLE
-- ==========================================================

CREATE TABLE IF NOT EXISTS rental_service.user_claims (
    id SERIAL PRIMARY KEY,
    user_id UUID NOT NULL,
    claim_type TEXT,
    claim_value TEXT,
    FOREIGN KEY (user_id) REFERENCES rental_service.users(id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS ix_user_claims_user_id ON rental_service.user_claims (user_id);


-- ==========================================================
-- USER_LOGINS TABLE
-- ==========================================================

CREATE TABLE IF NOT EXISTS rental_service.user_logins (
    login_provider VARCHAR(128) NOT NULL,
    provider_key VARCHAR(128) NOT NULL,
    provider_display_name TEXT,
    user_id UUID NOT NULL,
    PRIMARY KEY (login_provider, provider_key),
    FOREIGN KEY (user_id) REFERENCES rental_service.users(id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS ix_user_logins_user_id ON rental_service.user_logins (user_id);


-- ==========================================================
-- USER_ROLES TABLE
-- ==========================================================

CREATE TABLE IF NOT EXISTS rental_service.user_roles (
    user_id UUID NOT NULL,
    role_id UUID NOT NULL,
    PRIMARY KEY (user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES rental_service.users(id) ON DELETE CASCADE,
    FOREIGN KEY (role_id) REFERENCES rental_service.roles(id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS ix_user_roles_role_id ON rental_service.user_roles (role_id);


-- ==========================================================
-- USER_TOKENS TABLE
-- ==========================================================

CREATE TABLE IF NOT EXISTS rental_service.user_tokens (
    user_id UUID NOT NULL,
    login_provider VARCHAR(128) NOT NULL,
    name VARCHAR(128) NOT NULL,
    value TEXT,
    PRIMARY KEY (user_id, login_provider, name),
    FOREIGN KEY (user_id) REFERENCES rental_service.users(id) ON DELETE CASCADE
);


-- ==========================================================
-- DRIVER_LICENSE_TYPE TABLE
-- ==========================================================

CREATE TABLE IF NOT EXISTS rental_service.driver_license_type (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    code VARCHAR(5) NOT NULL,
    description VARCHAR(100) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),

    CONSTRAINT uq_license_code UNIQUE (code)
);

CREATE INDEX IF NOT EXISTS idx_license_type_code ON rental_service.driver_license_type (code);


-- ==========================================================
-- COURIER TABLE
-- ==========================================================

CREATE TABLE IF NOT EXISTS rental_service.courier (
    id UUID PRIMARY KEY,
    full_name VARCHAR(100) NOT NULL,
    cnpj VARCHAR(18) NOT NULL UNIQUE,
    birth_date DATE NOT NULL,
    driver_license_number VARCHAR(20) NOT NULL UNIQUE,
    driver_license_type_id UUID NOT NULL REFERENCES rental_service.driver_license_type(id)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,
    driver_license_image_url TEXT,
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_driver_name ON rental_service.courier (full_name);


-- ==========================================================
-- MOTORCYCLE TABLE
-- ==========================================================

CREATE TABLE IF NOT EXISTS rental_service.motorcycle (
    id UUID PRIMARY KEY,
    year INTEGER NOT NULL CHECK (year > 2000),
    model VARCHAR(50) NOT NULL,
    plate VARCHAR(10) NOT NULL UNIQUE,
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_motorcycle_model ON rental_service.motorcycle (model);


-- ==========================================================
-- RENTAL_PLAN TABLE
-- ==========================================================

CREATE TABLE IF NOT EXISTS rental_service.rental_plan (
    id UUID PRIMARY KEY,
    days INTEGER NOT NULL CHECK (days > 0),
    daily_rate NUMERIC(10,2) NOT NULL CHECK (daily_rate > 0),
    penalty_percent NUMERIC(5,2) NOT NULL DEFAULT 0,
    description VARCHAR(100),
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),

    CONSTRAINT uq_rental_plan_days UNIQUE (days)
);

CREATE INDEX IF NOT EXISTS idx_plan_days ON rental_service.rental_plan (days);


-- ==========================================================
-- RENTAL TABLE
-- ==========================================================

CREATE TABLE IF NOT EXISTS rental_service.rental (
    id UUID PRIMARY KEY,
    driver_id UUID NOT NULL REFERENCES rental_service.courier(id)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,
    motorcycle_id UUID NOT NULL REFERENCES rental_service.motorcycle(id)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,
    plan_id UUID NOT NULL REFERENCES rental_service.rental_plan(id)
        ON DELETE RESTRICT
        ON UPDATE CASCADE,
    start_date DATE NOT NULL,
    expected_end_date DATE NOT NULL,
    end_date DATE,
    daily_value NUMERIC(10,2),
    penalty_value NUMERIC(10,2),
    total_value NUMERIC(10,2),
    status VARCHAR(20) NOT NULL CHECK (status IN ('ACTIVE', 'FINISHED', 'CANCELLED')),
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),

    CONSTRAINT uq_active_rental UNIQUE (motorcycle_id, status)
);


-- ==========================================================
-- NOTIFICATION TABLE
-- ==========================================================

CREATE TABLE IF NOT EXISTS rental_service.notification (
    id UUID PRIMARY KEY,
    event_type VARCHAR(100) NOT NULL,
    payload JSONB NOT NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'PENDING'
        CHECK (status IN ('PENDING', 'SENT', 'ERROR')),
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    sent_at TIMESTAMP
);


-- ==========================================================
-- INSERT USERS 
-- ==========================================================

INSERT INTO rental_service.users 
(id, user_name, normalized_username, email, normalized_email, email_confirmed, password_hash, security_stamp, concurrency_stamp, phone_number_confirmed, two_factor_enabled, lockout_enabled, access_failed_count)
VALUES
(gen_random_uuid(), 'admin@rental.com', 'ADMIN@RENTAL.COM', 'admin@rental.com', 'ADMIN@RENTAL.COM', true, 'AQAAAAIAAYagAAAAEHxCbOqEDXrSFqZQSCfynqxY2wtgr1ElLdSyLrGfX9T8RxdGaffs29y6s7ucsX1Q3g==', gen_random_uuid(), gen_random_uuid(), false, false, false, 0),
(gen_random_uuid(), 'manager@rental.com', 'MANAGER@RENTAL.COM', 'manager@rental.com', 'MANAGER@RENTAL.COM', true, 'AQAAAAIAAYagAAAAEO1UnZOn8v4us/0Mp839/lbVwFwMB3NnKk0mx4bB4ITTEjYmGshkUgCTGqgsgr+TSA==', gen_random_uuid(), gen_random_uuid(), false, false, false, 0),
(gen_random_uuid(), 'user@rental.com', 'USER@RENTAL.COM', 'user@rental.com', 'USER@RENTAL.COM', true, 'AQAAAAIAAYagAAAAEO0lQEG6wMBE9buJqO0vIQlq6Hkv9rYbhjTGYNaoC0osDAHR2K5dTZB05+H8SluQqA==', gen_random_uuid(), gen_random_uuid(), false, false, false, 0);


-- ==========================================================
-- INSERT ROLES
-- ==========================================================

INSERT INTO rental_service.roles (id, name, normalized_name, concurrency_stamp)
VALUES 
  (gen_random_uuid(), 'Admin', 'ADMIN', gen_random_uuid()),
  (gen_random_uuid(), 'Manager', 'MANAGER', gen_random_uuid()),
  (gen_random_uuid(), 'User', 'USER', gen_random_uuid());


-- ==========================================================
-- INSERT USER_ROLES 
-- ==========================================================

INSERT INTO rental_service.user_roles (user_id, role_id)
VALUES (
  (SELECT id FROM rental_service.users WHERE email = 'admin@rental.com'),
  (SELECT id FROM rental_service.roles WHERE name = 'Admin')
);

INSERT INTO rental_service.user_roles (user_id, role_id)
VALUES (
  (SELECT id FROM rental_service.users WHERE email = 'manager@rental.com'),
  (SELECT id FROM rental_service.roles WHERE name = 'Manager')
);

INSERT INTO rental_service.user_roles (user_id, role_id)
VALUES (
  (SELECT id FROM rental_service.users WHERE email = 'user@rental.com'),
  (SELECT id FROM rental_service.roles WHERE name = 'User')
);


-- ==========================================================
-- INSERT DRIVER_LICENSE_TYPE
-- ==========================================================

INSERT INTO rental_service.driver_license_type (id, code, description)
VALUES 
    (gen_random_uuid(), 'A',  'Motorcycle'),
    (gen_random_uuid(), 'B',  'Car'),
    (gen_random_uuid(), 'AB', 'Motorcycle and Car');
