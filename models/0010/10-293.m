A1 : C2
	| C3
	| A4 ;

A4 : C5
	| C6
	| C7
	| C8
	| C9
	| C10 ;

%%

not (not (not C7 implies C9) and A4 implies not C3) or not C3 ;

