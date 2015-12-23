A1_ : A1+ :: _A1 ;

A1 : A2+ :: A2_
	| A3
	| A4+ :: A4_ ;

A2 : C5
	| A6+ :: A6_
	| C7
	| C27 C28 :: A8
	| C9
	| C11 [C12] A13* [C14] C15 :: A10 ;

A6 : C35
	| C36
	| C37
	| C38
	| C39
	| C43 [C44] C45 [C46] [C47] :: A40
	| C48 [C49] C50 :: A41
	| C42 ;

A13 : C16
	| C17
	| C18 ;

A3 : C19
	| A20
	| C21
	| C22
	| C23
	| C24
	| C25
	| C26 ;

A20 : C29
	| C30
	| C31
	| C32 ;

A4 : C33
	| C34 ;

%%

not (not A40 implies not C42) implies C9 iff C42 ;
not (not C23 and not C16) and not C21 implies A8 ;
((C37 iff C12) implies not C35) implies not C36 ;
(not (not C30 or C33) iff C22) and not C15 ;
not (not (C11 iff C42 iff not C43) or not C5) ;

