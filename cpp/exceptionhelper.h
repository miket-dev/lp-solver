#pragma once

#include <stdexcept>

class NotImplementedException : public std::exception
{
private:
    std::string msg;

public:
    NotImplementedException(const std::string& message = "") : msg(message)
    {
    }

    const char * what() const throw()
    {
        return msg.c_str();
    }
};

class ApplicationException : public std::exception
{
private:
    std::string msg;

public:
    ApplicationException(const std::string& message = "") : msg(message)
    {
    }

    const char * what() const throw()
    {
        return msg.c_str();
    }
};
