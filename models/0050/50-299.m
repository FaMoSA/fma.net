A1_ : A1+ :: _A1 ;

A1 : C3 A4 [A5] [C6] [C7] C8 C9 :: A2 ;

A4 : C15 C16 C17 C18 C19 [A20] C21 [C22] C23 :: _A4 ;

A20 : C24
	| C25
	| C26
	| C27
	| A28
	| C29
	| C30
	| C31
	| C32
	| C33 ;

A28 : C34
	| C35
	| C36
	| C37 ;

A5 : C38 [C39] [A40] [C41] [C42] [C43] C44 C45 [C46] :: A10
	| C11
	| C12
	| C13
	| C14 ;

A40 : C47 [C48] [C49] [C50] :: _A40 ;

%%

((C39 iff not A10) or not C14 iff C21) and not C47 ;
not (not C22 and C37 implies C14) ;
(C15 and C32 implies not C17) implies A2 ;
A40 implies C48 ;
not ((not A1 implies C49) implies A2 iff C44) ;

