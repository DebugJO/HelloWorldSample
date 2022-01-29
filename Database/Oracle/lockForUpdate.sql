-- 10개 중 lock 없는 곳에 하나 접속 시도(10개가 동시에 전송)
-- col1 col2
-- 10   AA
-- 20   AB *lock 유도(다른 접속은 아래에서 접수 하도록...)
-- 20   AB
-- 20   CN
-- ...
-- 20   SK
-- ...
-- 20... 10개

select col1, col2 from AT where col1 = '20' and rownum = 1 for update; -- 문제발생

-- 해결쿼리 (시퀀스 이용, 10개 생성)
create SEQUENCE seq
start with 1
increment by 1
maxvalue 10
cycle
cache 9 

-- 최종쿼리
select seq.nextval into :A from dual;
select rnum, col1, col2
from(select rownum as rnum, col1, col2 from AT where col1 = '20')
where rnum = A1 and rownum = 1;
