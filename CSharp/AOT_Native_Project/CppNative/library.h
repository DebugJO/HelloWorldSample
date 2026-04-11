#pragma once
#include <string>
#include <array>

template<size_t N>
class ObfuscatedString {
public:
    template<size_t... Is>
    constexpr ObfuscatedString(const char *str, char key, std::index_sequence<Is...>)
        : m_key(key), m_data{static_cast<char>(str[Is] ^ key)...} {
    }

    [[nodiscard]] std::string decrypt() const {
        std::string result;
        result.reserve(N);
        for (size_t i = 0; i < N; ++i) {
            result += static_cast<char>(m_data[i] ^ m_key);
        }
        return result;
    }

private:
    char m_key;
    std::array<char, N> m_data;
};

#define OBFUSCATE(str) ([]() { \
    constexpr size_t _len = sizeof(str); \
    return ObfuscatedString<_len>(str, 0x5A, std::make_index_sequence<_len>{}); \
}())

#if defined(_WIN32)
#define MY_API __declspec(dllexport)
#else
#define MY_API __attribute__((visibility("default")))
#endif

extern "C" {
MY_API int GetSecretData(char *buffer, int obfuscatedKey);
}
