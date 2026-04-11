#include "library.h"
#include <iostream>

#define FIXED_BUFFER_SIZE 1024
#define SALT 0x1234
#define VALID_KEY 0x7777

int GetSecretData(char *buffer, const int obfuscatedKey) {
    if ((obfuscatedKey ^ SALT) != VALID_KEY) {
        return -1;
    }

    static ObfuscatedString<sizeof ("cpp 보안 문자열")> secret = OBFUSCATE("cpp 보안 문자열");
    const std::string decrypted = secret.decrypt();

    if (buffer != nullptr) {
        const size_t len = (decrypted.length() < FIXED_BUFFER_SIZE) ? decrypted.length() : FIXED_BUFFER_SIZE - 1;
        memcpy(buffer, decrypted.c_str(), len);
        buffer[len] = '\0';
        return static_cast<int>(len);
    }
    return 0;
}
