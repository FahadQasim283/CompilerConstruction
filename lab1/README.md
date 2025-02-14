# 📂 Lab 1

This folder contains two C# programs related to password validation and generation.

## Files

- `password_validator.cs`: A program to validate passwords based on specific criteria.
- `password_generator.cs`: A program to generate passwords based on user input.

---

## password_validator.cs

This program validates passwords according to the following criteria:
- Must be between 1 and 12 characters.
- Must contain at least 2 of the digits 2, 0, 3, or 4.
- Must contain at least 1 uppercase letter.
- Must contain at least 2 lowercase letters.
- Must contain at least 2 special characters.
- Must contain at least 4 of the letters 'f', 'a', 'h', 'd', 'q', 's', 'i', or 'm'.
- Must start with a letter.

### Usage

Run the program to validate a predefined list of passwords. The output will indicate whether each password is valid or invalid.

---

## password_generator.cs

This program generates a password based on the following user input:
- First name
- Last name
- Registration number
- Favorite movie
- Favorite food

### Criteria

- Use the entire first name.
- Append 3 characters from the last name (if the last name is shorter than 3 characters, use all of it).
- Append 3 digits extracted from the registration number (digits are extracted in the order they appear; if fewer than 3 digits exist, the program warns and exits).
- Append 2 characters from the favorite movie (if the movie string is shorter than 2 characters, use all of it).
- Append 1 character from the favorite food (if empty, the program warns and exits).

### Usage

Run the program to generate a password based on the predefined input. The generated password will be printed to the console.

---

## 📄 License

This project is Public no License required. Make sure credit author by mentioning GitHub while using this project.

---

## 📧 Contact

For inquiries or support, open an issue on the GitHub repository.

---

**⭐️ If you like this project, don't forget to give it a star!** 😊
