# 🪪 DVLD - Driving & Vehicle Licensing Department (C# WinForms – 3-Tier Architecture)

## 📖 About the Project
A comprehensive Windows Forms desktop application designed to manage the entire lifecycle of driving licenses. This project is a deep dive into **3-Tier Architecture**, handling complex business rules, relational database integrity, and high-level UI customization.

The system manages everything from person registration and user authentication to the intricate process of multi-stage testing (Vision, Written, Street) and license issuance (Local and International). It also includes specialized modules for license renewals, replacements, and a full Detention/Release system.

This project is a major milestone in the **Abu-Hadhoud Platform** training track with **Dr. Muhammed Abu-Hadhoud**, representing a significant leap in architectural complexity and data management from previous projects.

## 📌 Features

### 1. 📝 Applications & Licensing Services
* **New Driving License:** Full workflow for both **Local** and **International** license issuance.
* **License Lifecycle:** Automated handling for **Renewals** and **Replacements** (Lost or Damaged).
* **Release Detained License:** Integrated workflow to pay fines and restore license status.
* **Retake Test:** Comprehensive logic for managing failed tests and re-application fees.

### 2. 🗂️ Manage Applications
* **Local Licenses:** Full flow from Application -> Vision Test -> Written Test -> Street Test -> Issuance.
* **International Driving License Applications:** Management of global permits linked to local records.
* **Application & Test Types:** Full CRUD for managing system-wide application fees and test parameters.

### 3. ⚖️ Detain Licenses
* **Manage Detained Licenses:** Complete history and current status of all seized licenses.
* **Detain License:** Secure module to seize licenses with custom fine amounts.
* **Release Detained License:** Direct link between payment applications and detention records.

### 4. 👥 People, Drivers & Users
* **People:** Core module for registry, search, and personal data handling.
* **Drivers:** Specialized layer that activates once a person is issued their first license.
* **Users:** Full CRUD for system users with security logging.

### 5. ⚙️ Account Settings
* **User Info:** View current logged-in user details.
* **Change Password:** Secure password update mechanism.
* **Sign Out:** Safe session termination.

## 🏗️ Architecture & Technical Highlights
* **3-Tier Architecture:** Complete separation of **Presentation (UI)**, **Business Logic (BLL)**, and **Data Access (DAL)** layers.
* **Data Integrity:** Advanced SQL logic (ISNULL, CAST, Complex Joins) to prevent orphaned records and ensure relational safety.
* **Custom UI Aesthetics:** Implementation of a custom **Purple/Dark Theme** using `ProfessionalRenderer` and `ColorTable` classes for a modern feel.
* **Memory Management:** Efficient object lifecycle handling and optimized data fetching.

## 🛠️ Technologies Used
* **C#**
* **.NET Framework**
* **Windows Forms (WinForms)**
* **ADO.NET**
* **SQL Server**
* **Relational Database Design**

---
*Started: February 20, 2026 | Completed: April 1, 2026*
