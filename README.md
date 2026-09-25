# 🏦 Mock Banking Application

A demonstration banking system that allows users to create accounts, manage multiple bank accounts, and perform financial transactions. Users can deposit money, withdraw funds, and transfer money between accounts, with all transactions logged for audit purposes. The application includes role-based access control with admin capabilities and supports checking accounts with overdraft features.

## Database Schema

This project uses a relational database with six main entities: **Login** (user authentication), **Account** (core account data linked to users), **Checkbook** (checkbook linkage per account), **Checking** (overdraft-specific settings), **Transaction** (all money movements with amount and date), and **TransferTransaction** (transfer-specific destination tracking). The schema follows a one-to-many relationship pattern where one user can own multiple accounts, and each account can have many associated transactions. This is a demo project for educational purposes.

<img width="1235" height="692" alt="image" src="https://github.com/user-attachments/assets/65593eab-94a6-4ff9-9434-2be1cda11f0a" />
