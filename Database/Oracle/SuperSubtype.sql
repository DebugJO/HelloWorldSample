select
/*+ push_subq index(세금계산 세금게산_PK) */
	세금계산.번호,
	세금계산.발행일자,
	세금계산.총액,
	세금계산.세액,
	decode(세금계산.choice1, '고객', to_char(세금계산.고객_번호),
		세금계산.대리점_번호||to_char(세금계산.기타업체_번호)) 고객번호,
	세금계산.고객_상호
from 세금계산
	in(select /*+ use_nl(지로집금, 세금.계산_지로집금) */ 세금계산_지로집금.세금계산_번호
		from 지로집금, 세금계산_지로집금
		where 세금계산_지로집금.지로집금_번호 = 지로집금.번호
			and 지로집금.일자 between 'from_date' and 'to_date'
	and exist(select 1 from 고객 
				where 세금계산.choice1 = '고객'
					and 고객.번호=세금계산.고객_번호
					and 고객.지로대상영부 = 'Y'
					and 고객.청구시원_번호 = '19950113'
				union all
					select 1 from dual where 세금계산.choice1 = '대리점');
