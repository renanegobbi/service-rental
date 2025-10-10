-- Cria o schema
CREATE SCHEMA IF NOT EXISTS rental AUTHORIZATION postgres;

-- Tabela de RefreshTokens
CREATE TABLE IF NOT EXISTS rental."RefreshTokens" (
    "Id" uuid PRIMARY KEY,
    "Username" text,
    "Token" uuid NOT NULL,
    "ExpirationDate" timestamptz NOT NULL
);

-- Tabela de Roles
CREATE TABLE IF NOT EXISTS rental."Roles" (
    "Id" uuid PRIMARY KEY,
    "Name" varchar(256),
    "NormalizedName" varchar(256) UNIQUE,
    "ConcurrencyStamp" text
);

-- Tabela de Users
CREATE TABLE IF NOT EXISTS rental."Users" (
    "Id" uuid PRIMARY KEY,
    "UserName" varchar(256),
    "NormalizedUserName" varchar(256) UNIQUE,
    "Email" varchar(256),
    "NormalizedEmail" varchar(256),
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text,
    "SecurityStamp" text,
    "ConcurrencyStamp" text,
    "PhoneNumber" text,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamptz,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" int NOT NULL
);

-- Tabela RoleClaims
CREATE TABLE IF NOT EXISTS rental."RoleClaims" (
    "Id" SERIAL PRIMARY KEY,
    "RoleId" uuid NOT NULL,
    "ClaimType" text,
    "ClaimValue" text,
    CONSTRAINT "FK_RoleClaims_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES rental."Roles"("Id") ON DELETE CASCADE
);

-- Tabela UserClaims
CREATE TABLE IF NOT EXISTS rental."UserClaims" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" uuid NOT NULL,
    "ClaimType" text,
    "ClaimValue" text,
    CONSTRAINT "FK_UserClaims_Users_UserId" FOREIGN KEY ("UserId") REFERENCES rental."Users"("Id") ON DELETE CASCADE
);

-- Tabela UserLogins
CREATE TABLE IF NOT EXISTS rental."UserLogins" (
    "LoginProvider" varchar(128) NOT NULL,
    "ProviderKey" varchar(128) NOT NULL,
    "ProviderDisplayName" text,
    "UserId" uuid NOT NULL,
    PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_UserLogins_Users_UserId" FOREIGN KEY ("UserId") REFERENCES rental."Users"("Id") ON DELETE CASCADE
);

-- Tabela UserRoles
CREATE TABLE IF NOT EXISTS rental."UserRoles" (
    "UserId" uuid NOT NULL,
    "RoleId" uuid NOT NULL,
    PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_UserRoles_Users_UserId" FOREIGN KEY ("UserId") REFERENCES rental."Users"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserRoles_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES rental."Roles"("Id") ON DELETE CASCADE
);

-- Tabela UserTokens
CREATE TABLE IF NOT EXISTS rental."UserTokens" (
    "UserId" uuid NOT NULL,
    "LoginProvider" varchar(128) NOT NULL,
    "Name" varchar(128) NOT NULL,
    "Value" text,
    PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_UserTokens_Users_UserId" FOREIGN KEY ("UserId") REFERENCES rental."Users"("Id") ON DELETE CASCADE
);

-- Índices
CREATE INDEX IF NOT EXISTS "EmailIndex" ON rental."Users"("NormalizedEmail");
CREATE INDEX IF NOT EXISTS "UserNameIndex" ON rental."Users"("NormalizedUserName");
CREATE INDEX IF NOT EXISTS "IX_RoleClaims_RoleId" ON rental."RoleClaims"("RoleId");
CREATE INDEX IF NOT EXISTS "IX_UserClaims_UserId" ON rental."UserClaims"("UserId");
CREATE INDEX IF NOT EXISTS "IX_UserLogins_UserId" ON rental."UserLogins"("UserId");
CREATE INDEX IF NOT EXISTS "IX_UserRoles_RoleId" ON rental."UserRoles"("RoleId");

-- Inserts iniciais
INSERT INTO rental."Roles" ("Id", "Name", "NormalizedName") 
VALUES ('d285a475-065e-4123-9b52-04af01e24b13', 'Admin', 'ADMIN')
ON CONFLICT DO NOTHING;

INSERT INTO rental."Users" ("Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount")
VALUES 
('eec3d9a2-b446-46d2-a497-9b21f8191164', 'user1@example.com', 'USER1@EXAMPLE.COM', 'user1@example.com', 'USER1@EXAMPLE.COM', false, 'AQAAAAIAAYagAAAAEGDOhmb1uGoshmAl3ptYbX8ktFS+nvtI70lV3OXWFEOUp/2o0Ngn5rCCdM1KftCQtA==', 'XHHH4XKMAYKYLAIAC45NUO4VYB4PFBXE', '49435fc0-0181-496f-94e2-cf5dc92bf331', false, false, true, 0),
('af79c79c-9d5c-4617-a048-dd21b20f4a2e', 'user2@example.com', 'USER2@EXAMPLE.COM', 'user2@example.com', 'USER2@EXAMPLE.COM', true, 'AQAAAAIAAYagAAAAEMOVlaF2jCxybBAslRx5tgz8K85kZMGCwG1fl6imb374YVSlWRarg7e1AfOEqg19LQ==', 'XCGJY2YXZYMWX3M4HYW4CDRTA22V5BSJ', 'ae293275-d850-43b0-b46d-2967086afd52', false, false, true, 0)
ON CONFLICT DO NOTHING;

INSERT INTO rental."RefreshTokens" ("Id", "Username", "Token", "ExpirationDate") 
VALUES 
('9b10d122-1faa-4d36-bed3-ccc8bd242b1f','user1@example.com','27322dff-3cde-401f-9aa3-964acb3b3984','2025-10-01 17:18:12-03'),
('7c3e8099-bac7-435a-9a45-239987b8be3f','user2@example.com','2c66d797-e904-435b-bdb7-b1f9fccf291b','2025-10-01 22:49:28-03')
ON CONFLICT DO NOTHING;






-- Cria o schema
CREATE SCHEMA IF NOT EXISTS rental AUTHORIZATION postgres;

-- Tabela de RefreshTokens
CREATE TABLE IF NOT EXISTS rental."RefreshTokens" (
    "Id" uuid PRIMARY KEY,
    "Username" text,
    "Token" uuid NOT NULL,
    "ExpirationDate" timestamptz NOT NULL
);

-- Tabela de Roles
CREATE TABLE IF NOT EXISTS rental."Roles" (
    "Id" uuid PRIMARY KEY,
    "Name" varchar(256),
    "NormalizedName" varchar(256) UNIQUE,
    "ConcurrencyStamp" text
);

-- Tabela de Users
CREATE TABLE IF NOT EXISTS rental."Users" (
    "Id" uuid PRIMARY KEY,
    "UserName" varchar(256),
    "NormalizedUserName" varchar(256) UNIQUE,
    "Email" varchar(256),
    "NormalizedEmail" varchar(256),
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text,
    "SecurityStamp" text,
    "ConcurrencyStamp" text,
    "PhoneNumber" text,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamptz,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" int NOT NULL
);

-- Tabela RoleClaims
CREATE TABLE IF NOT EXISTS rental."RoleClaims" (
    "Id" SERIAL PRIMARY KEY,
    "RoleId" uuid NOT NULL,
    "ClaimType" text,
    "ClaimValue" text,
    CONSTRAINT "FK_RoleClaims_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES rental."Roles"("Id") ON DELETE CASCADE
);

-- Tabela UserClaims
CREATE TABLE IF NOT EXISTS rental."UserClaims" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" uuid NOT NULL,
    "ClaimType" text,
    "ClaimValue" text,
    CONSTRAINT "FK_UserClaims_Users_UserId" FOREIGN KEY ("UserId") REFERENCES rental."Users"("Id") ON DELETE CASCADE
);

-- Tabela UserLogins
CREATE TABLE IF NOT EXISTS rental."UserLogins" (
    "LoginProvider" varchar(128) NOT NULL,
    "ProviderKey" varchar(128) NOT NULL,
    "ProviderDisplayName" text,
    "UserId" uuid NOT NULL,
    PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_UserLogins_Users_UserId" FOREIGN KEY ("UserId") REFERENCES rental."Users"("Id") ON DELETE CASCADE
);

-- Tabela UserRoles
CREATE TABLE IF NOT EXISTS rental."UserRoles" (
    "UserId" uuid NOT NULL,
    "RoleId" uuid NOT NULL,
    PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_UserRoles_Users_UserId" FOREIGN KEY ("UserId") REFERENCES rental."Users"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_UserRoles_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES rental."Roles"("Id") ON DELETE CASCADE
);

-- Tabela UserTokens
CREATE TABLE IF NOT EXISTS rental."UserTokens" (
    "UserId" uuid NOT NULL,
    "LoginProvider" varchar(128) NOT NULL,
    "Name" varchar(128) NOT NULL,
    "Value" text,
    PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_UserTokens_Users_UserId" FOREIGN KEY ("UserId") REFERENCES rental."Users"("Id") ON DELETE CASCADE
);

-- Índices
CREATE INDEX IF NOT EXISTS "EmailIndex" ON rental."Users"("NormalizedEmail");
CREATE INDEX IF NOT EXISTS "UserNameIndex" ON rental."Users"("NormalizedUserName");
CREATE INDEX IF NOT EXISTS "IX_RoleClaims_RoleId" ON rental."RoleClaims"("RoleId");
CREATE INDEX IF NOT EXISTS "IX_UserClaims_UserId" ON rental."UserClaims"("UserId");
CREATE INDEX IF NOT EXISTS "IX_UserLogins_UserId" ON rental."UserLogins"("UserId");
CREATE INDEX IF NOT EXISTS "IX_UserRoles_RoleId" ON rental."UserRoles"("RoleId");

-- Inserts iniciais
INSERT INTO rental."Roles" ("Id", "Name", "NormalizedName") 
VALUES ('d285a475-065e-4123-9b52-04af01e24b13', 'Admin', 'ADMIN')
ON CONFLICT DO NOTHING;

INSERT INTO rental."Users" ("Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount")
VALUES 
('eec3d9a2-b446-46d2-a497-9b21f8191164', 'user1@example.com', 'USER1@EXAMPLE.COM', 'user1@example.com', 'USER1@EXAMPLE.COM', false, 'AQAAAAIAAYagAAAAEGDOhmb1uGoshmAl3ptYbX8ktFS+nvtI70lV3OXWFEOUp/2o0Ngn5rCCdM1KftCQtA==', 'XHHH4XKMAYKYLAIAC45NUO4VYB4PFBXE', '49435fc0-0181-496f-94e2-cf5dc92bf331', false, false, true, 0),
('af79c79c-9d5c-4617-a048-dd21b20f4a2e', 'user2@example.com', 'USER2@EXAMPLE.COM', 'user2@example.com', 'USER2@EXAMPLE.COM', true, 'AQAAAAIAAYagAAAAEMOVlaF2jCxybBAslRx5tgz8K85kZMGCwG1fl6imb374YVSlWRarg7e1AfOEqg19LQ==', 'XCGJY2YXZYMWX3M4HYW4CDRTA22V5BSJ', 'ae293275-d850-43b0-b46d-2967086afd52', false, false, true, 0)
ON CONFLICT DO NOTHING;

INSERT INTO rental."RefreshTokens" ("Id", "Username", "Token", "ExpirationDate") 
VALUES 
('9b10d122-1faa-4d36-bed3-ccc8bd242b1f','user1@example.com','27322dff-3cde-401f-9aa3-964acb3b3984','2025-10-01 17:18:12-03'),
('7c3e8099-bac7-435a-9a45-239987b8be3f','user2@example.com','2c66d797-e904-435b-bdb7-b1f9fccf291b','2025-10-01 22:49:28-03')
ON CONFLICT DO NOTHING;




CREATE TABLE IF NOT EXISTS rental."Motorcycles" (
    "Id" UUID PRIMARY KEY,
    "Identifier" VARCHAR(100) NOT NULL,
    "Year" INT NOT NULL,
    "Model" VARCHAR(100) NOT NULL,
    "Plate" VARCHAR(20) NOT NULL UNIQUE,
	"CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT now(),
    "IsDeleted" BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE INDEX IF NOT EXISTS idx_motorcycles_plate ON rental."Motorcycles" ("Plate");
CREATE INDEX IF NOT EXISTS idx_motorcycles_isdeleted ON rental."Motorcycles" ("IsDeleted");

CREATE TABLE IF NOT EXISTS rental."Couriers" (
    "Id" uuid PRIMARY KEY,
    "Name" varchar(150) NOT NULL,
    "Cnpj" varchar(20) NOT NULL UNIQUE,
    "BirthDate" date NOT NULL,
    "CnhNumber" varchar(50) NOT NULL UNIQUE,
    "CnhType" varchar(5) NOT NULL,     -- A, B, A+B
    "CnhImagePath" text,               -- storage path (disk, S3, MinIO)
    "CreatedAt" timestamptz NOT NULL DEFAULT now()
);


CREATE TABLE IF NOT EXISTS rental."Rentals" (
    "Id" uuid PRIMARY KEY,
    "MotorcycleId" uuid NOT NULL,
    "CourierId" uuid NOT NULL,
    "PlanType" varchar(20) NOT NULL,   -- 7, 15, 30, 45, 50 days
    "DailyRate" decimal(10,2) NOT NULL,
    "StartDate" date NOT NULL,
    "ExpectedEndDate" date NOT NULL,
    "EndDate" date,
    "TotalValue" decimal(10,2),
    "CreatedAt" timestamptz NOT NULL DEFAULT now(),
    CONSTRAINT "FK_Rentals_Motorcycles" FOREIGN KEY ("MotorcycleId") REFERENCES rental."Motorcycles"("Id"),
    CONSTRAINT "FK_Rentals_Couriers" FOREIGN KEY ("CourierId") REFERENCES rental."Couriers"("Id")
);


CREATE TABLE IF NOT EXISTS rental."Orders" (
    "Id" uuid PRIMARY KEY,
    "Description" text NOT NULL,
    "CreatedAt" timestamptz NOT NULL DEFAULT now(),
    "AssignedCourierId" uuid,
    "Status" varchar(20) NOT NULL DEFAULT 'Pending', -- Pending, InProgress, Delivered
    CONSTRAINT "FK_Orders_Couriers" FOREIGN KEY ("AssignedCourierId") REFERENCES rental."Couriers"("Id")
);


CREATE TABLE IF NOT EXISTS rental."MotorcycleEventNotifications" (
    "Id" uuid PRIMARY KEY,
    "MotorcycleId" uuid NOT NULL,
    "Plate" varchar(20) NOT NULL,
    "Year" int NOT NULL,
    "NotifiedAt" timestamptz NOT NULL DEFAULT now()
);