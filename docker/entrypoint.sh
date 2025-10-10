#!/bin/bash
set -e

echo "⏳ Waiting for PostgreSQL to start..."
sleep 10

echo "📦 Running custom init scripts..."
psql -U "$POSTGRES_USER" -d "$POSTGRES_DB" -f /docker-entrypoint-initdb.d/init-database.sql

exec "$@"