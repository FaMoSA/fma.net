A1_ : A1+ :: _A1 ;

A1 : A2+ :: A2_
	| A3
	| C4 ;

A2 : C5
	| C6
	| C7
	| C8
	| C9
	| C11 [C12] A13* [C14] C15 :: A10 ;

A13 : C16
	| C17
	| C18 ;

A3 : C19
	| C20 ;

%%

not ((C4 implies C7) and C15) ;
not C5 and not C8 implies A2 ;

