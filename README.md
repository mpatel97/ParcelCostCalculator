# Parcel Cost Calculator

## Purpose

This library provides functionality to compute shipping costs for parcels in a single order, as part of the Back End Technical Test for First AML

## Changelog
- Phase 1: Base implentation complete to map the follow calculation rules:
    - Small parcel: all dimensions < 10cm. Cost $3
    - Medium parcel: all dimensions < 50cm. Cost $8
    - Large parcel: all dimensions < 100cm. Cost $15
    - XL parcel: any dimension >= 100cm. Cost $25 

## Assumptions
- At this stage, the calulations are straight-forward, so we're defining cost mapping within the calculation services (pricing service and order cost calculator)