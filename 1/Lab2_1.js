function numberToBinStr(n) {
    let negativeSign = n < 0
    n = Math.abs(n)
    return (negativeSign ? '-' : '') + n.toString(2).padStart(32, '0')
}

function multiply(n1, n2) {
    let result = 0;
    let negativeSign = (n1 > 0 && n2 < 0) || (n1 < 0 && n2 > 0)
    n1 = Math.abs(n1)
    n2 = Math.abs(n2)
    while (n2) {
        if (n2 & 1) {
            result += n1
        }
      result += n1;
        n1 <<= 1
        n2 >>= 1
    }
    if (negativeSign) {
        result = -result
    }
    return result
}

function multiplyWithLogs(n1, n2) {
    let result = 0;
    console.log('Число n1: ' + numberToBinStr(n1));
    console.log('Число n2: ' + numberToBinStr(n2));
    let negativeSign = (n1 > 0 && n2 < 0) || (n1 < 0 && n2 > 0)
    console.log('Знак: ' + (negativeSign ? '-' : '+'));


    n1 = Math.abs(n1)
    n2 = Math.abs(n2)

    console.log('positive n1: ' + numberToBinStr(n1));
    console.log('positive n2: ' + numberToBinStr(n2));

    while (n2) {
        console.log('n1: ' + numberToBinStr(n1));
        console.log('n2: ' + numberToBinStr(n2));
        if (n2 & 1) {
            result += n1
        }
        console.log('n1 <<= 1');
        n1 <<= 1
        console.log('n2 >>= 1');
        n2 >>= 1
        console.log('result: ' + numberToBinStr(result));
        console.log('next iteration\n');
    }
    console.log('if negativeSign than result = -result');
    if (negativeSign) {
        console.log('result = -result');
        result = -result
    }
    console.log('done, result: ' + result + ' or ' + numberToBinStr(result));

    return result
}

console.log(multiplyWithLogs(6,-8));