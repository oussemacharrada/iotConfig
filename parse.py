#!/usr/bin/env python3
# -*- coding: utf-8 -*-
# ----------------------------------------------------------------------------
# Copyright 2019 ARM Limited or its affiliates
#
# SPDX-License-Identifier: Apache-2.0
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
# ----------------------------------------------------------------------------
import cbor2 as cbor
import json
import itertools
import textwrap

from suit_tool.manifest import SUITEnvelope

def main(options):
    try:
        # Read the manifest wrapper
        decoded_cbor_wrapper = cbor.loads(options.manifest.read())
        print("Decoded CBOR wrapper:")
        print(decoded_cbor_wrapper)

        # Create a SUITEnvelope and populate it from the decoded CBOR data
        wrapper = SUITEnvelope().from_suit(decoded_cbor_wrapper)
        print("\nSUITEnvelope:")
        print(wrapper)

        print("SUITEnvelope Suit representation:")
        print(cbor.dumps(wrapper.to_suit(), canonical=True)),
        print("SUITEnvelope JSON representation:")
        print(json.dumps(wrapper.to_json(), indent=2))
        print("SUITEnvelope Debug representation:")
        debug_info = wrapper.to_debug('')
        print('\n'.join(itertools.chain.from_iterable(
                [textwrap.wrap(t, 70) for t in debug_info.split('\n')]
            )))

        return 0

    except Exception as e:
        print("An error occurred:", str(e))
        return 1

if __name__ == "__main__":
    # Add your command line argument parsing and options setup here
    # ...
    # options = ...

    # Call the main function with options
    main(options)
