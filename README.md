# Student Record Management System #
This is a CRUD system using ASP.NET with jQuery and AJAX for requesting data from endpoints. Object-Oriented Programming (OOP) was applied to manipulate the database with Object Relational Mapping (ORM). JSON Web Token (JWT) was used to generate an access token for the authorization header.

Sections:
* [User Page](#user-page)
* [Administrator Page](#administrator-page)
* [Faculty Page](#faculty-page)
* [Student Page](#student-page)
* [Sign In](#sign-in)
* [Update](#update)
* [Delete](#delete)
* [Search](#search)
* [Sign Out](#sign-out)
* [Change Admin Username or Password](#change-admin-username-or-password)

## User Page ##
There are three types of users: student, faculty, and administrator.
![user_page](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/user%20page.jpg)

## Administrator Page ##
The administrator is responsible for adding academic member accounts and basic information. The added accounts are used to enable academic members to sign in to the system.
![admin_page](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/administrator%20page.jpg)

## Faculty Page ##
The faculty is responsible for adding student records.
![faculty_page](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/faculty%20page.jpg)

## Student Page ##
The student is only allowed to see their own records.
![student_page](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/student%20page.jpg)

## Sign In ##
During sign-in, the server generates an access token, and the client-side accepts and stores this token temporarily.

### Admin ###
![admin_signin](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/admin%20-%20signin.jpg)

### Faculty ###
![faculty_signin](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/faculty-signin.jpg)

### Student ###
![student_signin](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/student-signin.jpg)

## Update ##
During an update, the server checks for data conflicts before updating the database.

### Update Member ###
![update_member](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/update%20member.jpg)

### Update Record ###
![update_record](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/update%20record.jpg)

## Delete ##
The system warns the user before performing the deletion.
![delete_record](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/delete.jpg)

## Search ##
The system checks for any possible keywords in the database and returns the filtered data.
![search](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/administrator%20search.jpg)

## Sign Out ##
The system clears the access token that was stored on the client-side.
![sign_out](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/signout.jpg)

## Change Admin Username or Password ##
You can change the admin username or password in the appsettings.json file.
![admin_chage_auth](https://github.com/phil1217/SRMS-Images/blob/6a0c8c17cef3c92a83db2ead1f5c2ff657e4a308/admin%20-%20auth.jpg)
