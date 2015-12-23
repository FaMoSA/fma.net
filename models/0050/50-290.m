A1 : C2 C3 A4 C5 C6 :: _A1 ;

A4 : C7 [C8] A9+ C10 [C11] [C12] A13 :: _A4 ;

A9 : A38+ :: A38_
	| C39
	| C40
	| C42 C43 [C44] :: A41 ;

A38 : C45
	| C46
	| C47
	| C48
	| C49
	| C50 ;

A13 : C14 C15 C16 [C17] A18 C19 C20 A21 :: _A13 ;

A18 : C31 C32 [C33] C34 C35 [C36] [C37] :: _A18 ;

A21 : C22 [C23] C24 C25 C26 C27 [C28] C29 C30 :: _A21 ;

%%

(C26 or C14) and not A38 ;
not ((C30 and C37 or not C36) and not C28) ;
((A38 and not C11 implies C31) implies C3) and not C46 ;
not (C2 implies A21) or C3 ;
(C30 implies not C36) implies C48 ;

