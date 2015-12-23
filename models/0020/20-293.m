A1 : C2
	| C3
	| A4 ;

A4 : C5
	| C6
	| C7
	| C8
	| A14 C15 C16 C17 C18 :: A9
	| C10
	| C12 [C13] :: A11 ;

A14 : C19 [C20] :: _A14 ;

%%

C13 implies C19 iff not C19 ;
C12 implies C7 ;

