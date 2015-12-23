A1 : A2 C3 [C4] C5 A6 C7 [C8] A9* A10 :: _A1 ;

A2 : C41 [C42] [C43] C44 C45 C46 [C47] C48 [C49] :: _A2 ;

A6 : C21
	| C22 ;

A9 : C11
	| A12+ :: A12_ ;

A12 : C23
	| C24
	| C32 C33 A34 [C35] C36 C37 :: A25
	| C26
	| C27
	| C28
	| C29
	| C30
	| C31 ;

A34 : C38
	| C39
	| C40 ;

A10 : C13 A14+ [C15] [C16] C17 C18 C19 [C20] :: _A10 ;

A14 : C50 ;

%%

not C18 iff not C29 ;
C31 and not C42 ;
C3 and not C35 iff not C40 ;
(A9 iff C48) and not C37 iff not C3 ;
(A12 iff not C44) or not C26 ;

