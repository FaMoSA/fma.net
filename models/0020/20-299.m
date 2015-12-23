A1_ : A1+ :: _A1 ;

A1 : C3 A4 [A5] [C6] [C7] C8 C9 :: A2 ;

A4 : C15 C16 C17 C18 C19 [C20] :: _A4 ;

A5 : C10
	| C11
	| C12
	| C13
	| C14 ;

%%

not ((not (not C13 or not C10) implies C17) implies not C15) ;
C7 and not A1 or C16 ;

