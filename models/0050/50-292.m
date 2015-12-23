A1 : A2 [A3] A4* [C5] [A6] :: _A1 ;

A2 : C7
	| C8 ;

A3 : C18
	| C19
	| C20 ;

A4 : C9
	| C10
	| A11+ :: A11_ ;

A11 : C16
	| C17 ;

A6 : C12 C13 C14 A15* :: _A6 ;

A15 : C33 A34* :: A21
	| C26 C27 [C28] C29 [C30] [C31] [C32] :: A22
	| C35 C36 [C37] C38 [C39] C40 C41 [C42] C43 C44 :: A23
	| C24
	| C25 ;

A34 : C45
	| C46
	| C47
	| C48
	| C49
	| C50 ;

%%

not ((not C45 or not C29 implies C17) or not C8) ;
not C45 and not A34 and not C38 ;
((C25 iff C27) implies A1) implies not C20 iff not A3 ;
not ((not A34 iff C17) or not C27) ;
A2 or not C9 or C27 ;

