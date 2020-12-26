select saledept,
	sum(decode(saledate, to_char(sysdate,'yyyymmdd'), sqlqty)), -- 당일분
	sum(decode(sign(8-(sysdate - to_date(saledate,'yyyymmdd'))),1,saleqty)), -- 1주일분
	substr(max(saledate||chulqty),9,15), --가장마지막 다량처리분
	sum9saleamt)
from mechul2t
where saledate like '199807%'
group by saledept
