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

A6 : C12 C13 C14 [C15] :: _A6 ;

%%

C5 and C18 ;
not A2 implies C16 ;

