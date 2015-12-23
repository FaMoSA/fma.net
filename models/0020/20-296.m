A1 : A2 C3 A4* [C5] C6 [C7] C8 [C9] C10 :: _A1 ;

A2 : C11 C12 :: _A2 ;

A4 : C13
	| C14
	| C15
	| C16
	| C17
	| C18
	| C19
	| C20 ;

%%

(not C3 or C9) and not A4 implies not C14 ;
not (not C18 or A1 or C18) implies A4 ;

