select lpad(chr(32), 2 * (level - 1)) || e.first_name as name,
       e.employee_id as empid,
       e.manager_id as mngid,
       level
from hr.employees e 
start with e.manager_id is null
connect by prior e.employee_id = e.manager_id;

select to_char(to_date('20201225') + level - 1, 'yyyyMMdd') from dual connect by level <= 100;

select to_char(max(dates), 'W') as a, 
       to_char(min(dates), 'W') as b,
       decode(to_char(dates, 'D'), 1, to_char(dates, 'IW') + 1, to_char(dates, 'IW')) as c,
    min(decode(to_char(dates, 'D'), 1, to_char(dates, 'DD'))) as 일,
    min(decode(to_char(dates, 'D'), 2, to_char(dates, 'DD'))) as 월,
    min(decode(to_char(dates, 'D'), 3, to_char(dates, 'DD'))) as 화,
    min(decode(to_char(dates, 'D'), 4, to_char(dates, 'DD'))) as 수,
    min(decode(to_char(dates, 'D'), 5, to_char(dates, 'DD'))) as 목,
    min(decode(to_char(dates, 'D'), 6, to_char(dates, 'DD'))) as 금,
    min(decode(to_char(dates, 'D'), 7, to_char(dates, 'DD'))) as 토
from (select (make_dates + level - 1) dates 
      from (select to_date('20201201', 'yyyyMMdd') make_dates from dual)
      connect by (make_dates + level - 1) <= last_day(make_dates))
group by decode(to_char(dates, 'D'), 1, to_char(dates, 'IW') + 1, to_char(dates, 'IW'))
order by decode(to_char(dates, 'D'), 1, to_char(dates, 'IW') + 1, to_char(dates, 'IW'));
