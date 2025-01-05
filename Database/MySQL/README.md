### MariaDB Sequence

```sql
create sequence seqTest
increment by 1
start with 100000
minvalue 100000
maxvalue 999999
cycle
-- cache

select nextval(seqTest)
select lastval(seqTest)
alter sequence seqTest restart 100000
```

## MariaDB, Mysql : my.cnf

8Core 16Thread 32Gbyte my.cnf 설정 : https://dreamsea77.tistory.com/319

```bash
# this is only for the mysqld standalone daemon
[mysqld]
# GENERAL
default_storage_engine = InnoDB
datadir = /data/var/mariadb-data
tmpdir = /data/var/mariadb-data/tmp
explicit_defaults_for_timestamp
lower_case_table_names = 1

###chracter
character-set-client-handshake=FALSE
init_connect = SET collation_connection = utf8_general_ci
init_connect = SET NAMES utf8
character-set-server = utf8
collation-server = utf8_general_ci

#dns query
skip-name-resolve
bind-address=0.0.0.0
skip-external-locking

# Disabling symbolic-links is recommended to prevent assorted security risks
symbolic-links  = 0

[mysqldump]
#default-character-set = utf8
max_allowed_packet = 1024M

## Connections
max_connections = 1000   #multiplier for memory usage via per-thread buffers
max_connect_errors   = 100   #default: 10
concurrent_insert  = 2       #default: 1, 2: enable insert for all instances
connect_timeout   = 60   #default -5.1.22: 5, +5.1.22: 10

## Table and TMP settings
max_heap_table_size = 1G #recommend same size as tmp_table_size
bulk_insert_buffer_size = 1G #recommend same size as tmp_table_size
tmp_table_size = 1G    #recommend 1G min

## Thread settings
thread_concurrency = 0   #recommend 2x CPU cores [0 create as many as needed]
thread_cache_size = 100  #recommend 5% of max_connections

# CACHES AND LIMITS
max_connections = 500
max_heap_table_size = 1G #recommend same size as tmp_table_size
bulk_insert_buffer_size = 1G #recommend same size as tmp_table_size
tmp_table_size= 1G    #recommend 1G min
open_files_limit = 65535
query_cache_size = 32M     #global buffer
query_cache_limit = 512K   #max query result size to put in cache
table_definition_cache = 4M
table_open_cache= 6000
innodb_open_files=6000
thread_cache_size = 50
tmp_table_size = 1G
thread_stack = 256K   #default: 32bit: 192K, 64bit: 256K
sort_buffer_size = 1M   #default: 2M, larger may cause perf issues
read_buffer_size = 1M   #default: 128K, change in increments of 4K
read_rnd_buffer_size = 1M   #default: 256K
join_buffer_size = 1M   #default: 128K

## InnoDB IO settings -  5.5.x and greater
innodb_write_io_threads = 16
innodb_read_io_threads = 16

# SAFETY
innodb = FORCE
innodb_strict_mode = 1
max_allowed_packet = 50M
max_connect_errors = 1000000
skip_name_resolve

# INNODB
innodb_data_file_path = ibdata1:128M;ibdata2:10M:autoextend
innodb_buffer_pool_size = 20G
innodb_buffer_pool_instances = 4
innodb_additional_mem_pool_size = 4M   #global buffer
innodb_flush_method = O_DIRECT     #O_DIRECT = local/DAS, O_DSYNC = SAN/iSCSI
innodb_flush_log_at_trx_commit = 0    #2/0 = perf, 1 = ACID
innodb_file_per_table = 1
innodb_log_files_in_group = 2
innodb_log_file_size = 512M
innodb_lock_wait_timeout = 300
innodb_thread_concurrency = 16   #recommend 2x core quantity
innodb_commit_concurrency = 16   #recommend 4x num disks
innodb_support_xa = 0       #recommend 0, disable xa to negate extra disk flush
innodb_io_capacity = 15000
skip-innodb-doublewrite

# LOGGING
log_error                      = /data/var/mariadb-data/logs/mysql-error.log
log_queries_not_using_indexes  = 1
slow_query_log                 = 1
slow_query_log_file            = /data/var/mariadb-data/logs/mysql-slow.log

# BINARY LOGGING
log_bin = /data/var/mariadb-data/logs/binlogs/mariadb-infaceone-bin
expire_logs_days = 14
max_binlog_size = 256M   #max size for binlog before rolling
binlog_cache_size = 64K   #default: 32K, size of buffer to hold TX queries
sync_binlog = 0
```

### 추가 설정 비교
```bash
skip-host-cache
skip-name-resolve
key_buffer_size = 256M
max_connections = 10000
innodb_buffer_pool_size = 2G # 메모리 50%

[mysqld]
transaction-isolation = REPEATABLE-READ #READ UNCOMMITTED, READ COMMITTED, REPEATABLE READ, SERIALIZABLE
```
