A1 : A2+ :: A2_
	| C7 A8+ [C9] :: A3
	| C5 A6* :: A4 ;

A2 : C20 ;

A8 : C10
	| C11
	| C12 ;

A6 : C13
	| C14
	| C15
	| C16
	| C17
	| C18
	| C19 ;

%%

not ((C7 or not C15) and C12) ;
not (not C5 implies not C5 iff C20) or not C15 ;

