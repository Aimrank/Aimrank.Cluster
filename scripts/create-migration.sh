#!/bin/bash

MIGRATION_NAME="$1"

dotnet ef migrations add $MIGRATION_NAME --startup-project ./src/Aimrank.Cluster.Api --project ./src/Aimrank.Cluster.Infrastructure
