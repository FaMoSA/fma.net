A1 : A2+ C3 C4 A5 C6 [C7] A8+ :: _A1 ;

A2 : C9
	| C10
	| C11
	| C12
	| C13
	| A14+ :: A14_ ;

A14 : C49
	| C50 ;

A5 : C15 C16 A17 C18 [A19] [A20] [C21] C22 :: _A5 ;

A17 : C41 [C42] [C43] [C44] C45 C46 [C47] [C48] :: _A17 ;

A19 : C27
	| C28
	| C29
	| C30
	| C31
	| C32 ;

A20 : C23 [C24] [C25] C26 :: _A20 ;

A8 : C33
	| C34
	| C35
	| C36
	| C37
	| C38
	| C39
	| C40 ;

%%

(not C36 implies not C7) or C22 ;
(not C50 iff not C25) implies C4 ;
not (not (not C46 iff not C25 iff C37) iff C24) ;
not C32 or not C33 implies not C35 ;
not (not C50 or C32 or not C16) or C23 implies A2 ;

