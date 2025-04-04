module m1 {
    requires m2;
    requires m3;
    exports p;
    uses p.Service;
}
