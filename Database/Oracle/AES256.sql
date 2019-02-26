--1. 해당유저 권한부여
GRANT EXECUTE ON DBMS_OBFUSCATION_TOOLKIT TO [유저명];
GRANT EXECUTE ON DBMS_CRYPTO TO [유저명];

--2. 패키지-head
create or replace PACKAGE CRYPTO_AES256
IS
        FUNCTION ENC_AES(input_string IN VARCHAR2)
                RETURN VARCHAR2;

        FUNCTION DEC_AES(encrypted_raw IN VARCHAR2)
                RETURN VARCHAR2;
END CRYPTO_AES256;

--3. 패키지-body
create or replace PACKAGE BODY CRYPTO_AES256
IS
        FUNCTION ENC_AES(input_string IN VARCHAR2)
                RETURN VARCHAR2
        IS
                return_base256 VARCHAR2(256);
                encrypted_raw RAW (2000);      -- encryption raw type date
                key_bytes_raw RAW (32);        -- encryption key (32raw => 32byte => 256bit)
                encryption_type PLS_INTEGER := -- encryption
                DBMS_CRYPTO.ENCRYPT_AES256 + DBMS_CRYPTO.CHAIN_CBC + DBMS_CRYPTO.PAD_PKCS5;
        BEGIN
                IF input_string       IS NOT NULL THEN
                        key_bytes_raw := UTL_I18N.STRING_TO_RAW('WKAF3xv7y,d5SZpzT8ftJR).shEQn#%@', 'AL32UTF8');
                        encrypted_raw := DBMS_CRYPTO.ENCRYPT ( src => UTL_I18N.STRING_TO_RAW(input_string, 'AL32UTF8'), typ => encryption_type, KEY => key_bytes_raw );
                        -- ORA-06502: PL/SQL: numeric or value error: hex to raw conversion error
                        return_base256 := UTL_RAW.CAST_TO_VARCHAR2(UTL_ENCODE.BASE64_ENCODE(encrypted_raw));
                END IF;
                RETURN return_base256;
        END ENC_AES;
     
        FUNCTION DEC_AES(encrypted_raw IN VARCHAR2)
                RETURN VARCHAR2
        IS
                output_string VARCHAR2 (200);
                decrypted_raw RAW (2000);      -- decryption raw type date
                key_bytes_raw RAW (32);        -- 256bit decryption key
                encryption_type PLS_INTEGER := -- decryption
                DBMS_CRYPTO.ENCRYPT_AES256 + DBMS_CRYPTO.CHAIN_CBC + DBMS_CRYPTO.PAD_PKCS5;
        BEGIN
                IF encrypted_raw      IS NOT NULL THEN
                        key_bytes_raw := UTL_I18N.STRING_TO_RAW('WKAF3xv7y,d5SZpzT8ftJR).shEQn#%@', 'AL32UTF8');
                        decrypted_raw := DBMS_CRYPTO.DECRYPT (
                        -- ORA-06502: PL/SQL: numeric or value error: hex to raw conversion error
                        src => UTL_ENCODE.BASE64_DECODE(UTL_RAW.CAST_TO_RAW(encrypted_raw)), typ => encryption_type, KEY => key_bytes_raw );
                        output_string := UTL_I18N.RAW_TO_CHAR(decrypted_raw, 'AL32UTF8');
                END IF;
                RETURN output_string;
        END DEC_AES;
END CRYPTO_AES256;

--4.사용법
SELECT CRYPTO_AES256.ENC_AES('컬럼명') FROM 테이블 명;
SELECT CRYPTO_AES256.DEC_AES('복호화 할 컬럼') FROM 테이블 명;

--5.출처 : https://yangyag.tistory.com/303?category=712845
