#!/bin/bash

docker build -t mariuszba/aimrank-cluster:$1 -f Dockerfile .
