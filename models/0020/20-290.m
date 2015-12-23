A1 : C2 C3 A4 C5 C6 :: _A1 ;

A4 : C7 [C8] C9 C10 [C11] [C12] A13 :: _A4 ;

A13 : C14 C15 C16 [C17] C18 C19 C20 :: _A13 ;

%%

not (not (C9 or C16) implies not A1) or C5 iff not C17 ;
not (((C7 implies not A13) implies not A4) implies not C18) ;

