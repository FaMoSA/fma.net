A1 : A2+ :: A2_
	| C7 A8+ [C9] :: A3
	| A5 A6* :: A4 ;

A2 : C20
	| C21
	| C22
	| C23
	| C24 ;

A8 : C10
	| C25 [C26] C27 A28+ C29 [C30] :: A11
	| A12+ :: A12_ ;

A28 : C31
	| C32
	| C33
	| C34 ;

A12 : C41 [C42] [C43] C44 [C45] C46 [C47] C48 [C49] [C50] :: A37
	| C38
	| C39
	| C40 ;

A5 : C35 C36 :: _A5 ;

A6 : C13
	| C14
	| C15
	| C16
	| C17
	| C18
	| C19 ;

%%

C30 and not C15 or A11 ;
not C34 and C26 ;
not ((not C16 iff not C31) and not C15 and C47) ;
((not (C34 iff C10) iff not C42) implies C21) and C39 ;
(not C49 implies not C14) and not C31 ;

