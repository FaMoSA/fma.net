A1 : C2 [A3] [A4] C5 [C6] A7 [A8] :: _A1 ;

A3 : C10 [C11] [C12] [C13] C14 :: _A3 ;

A4 : C15
	| C16
	| C17
	| C18
	| C19
	| C32 A33+ [C34] [C35] [C36] C37 C38 [C39] C40 [C41] :: A20 ;

A33 : C42
	| C46 C47 C48 [C49] [C50] :: A43
	| C44
	| C45 ;

A7 : C21 [C22] [C23] [C24] [A25] C26 [C27] C28 :: _A7 ;

A25 : C29 [C30] [C31] :: _A25 ;

A8 : C9 :: _A8 ;

%%

not ((not C10 implies not C30) and C12 or C29) ;
not (C48 or C9) and not C44 ;
not (not (not C36 implies C21) or C47 iff not A4) and not C31 ;
(not C48 iff A43) and C17 iff C24 ;
((not C19 iff C32) implies C16) implies C22 ;

