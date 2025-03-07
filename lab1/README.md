# 📂 Lab 1 – Password Validation & Generation in C#  

This folder contains two C# programs related to **password validation and generation**.  

## 📁 Files  

- 📝 **`password_validator.cs`** – Validates passwords based on specific criteria.  
- 📝 **`password_generator.cs`** – Generates passwords based on user input.
- 📝 **`output-password_generator.png`** – output screenshot
- 📝 **`output-password_validator.png`** – output screenshot

---

## 📝 password_validator.cs  

✔️ **Validation Criteria:**  
- Length: **1 to 12 characters**.  
- Must contain at least **2 digits** (from `2, 0, 3, 4`).  
- Must contain at least **1 uppercase letter**.  
- Must contain at least **2 lowercase letters**.  
- Must contain at least **2 special characters**.  
- Must include at least **4 letters** from (`f, a, h, d, q, s, i, m`).  
- Must **start** with a **letter**.  

📌 **Usage:**  
Run the program to validate a predefined list of passwords. The output will indicate whether each password is **valid ✅** or **invalid ❌**.  

---

## 📝 password_generator.cs  

✔️ **Password Generation Based on User Input:**  
- **First Name** – Used entirely.  
- **Last Name** – First **3 characters** (or all if shorter).  
- **Registration Number** – Extracts **3 digits** in order (if fewer than 3 exist, the program warns and exits).  
- **Favorite Movie** – First **2 characters** (or all if shorter).  
- **Favorite Food** – First **1 character** (if empty, the program warns and exits).  

📌 **Usage:**  
Run the program to generate a password based on the predefined input. The generated password will be **printed to the console**.  

---

## 📄 License  

🚀 **This project is public – No license required!**  
🔗 If you use this project, please **credit the author** by mentioning the **GitHub repository**.  

---

## 📧 Contact  

📮 For inquiries or support, **open an issue** on the GitHub repository.  

---

🌟 **If you like this project, don't forget to give it a star!** ⭐😊  

Let me know if you need any further refinements! 🚀
