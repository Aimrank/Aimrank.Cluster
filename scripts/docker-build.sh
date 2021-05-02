#!/bin/bash

docker build -t ghcr.io/aimrank/aimrank-cluster:$1 -f Dockerfile .
